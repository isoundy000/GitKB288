using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using SseManageClass;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using BCW.SSE.Class.sseMgr;


public partial class Manage_game_sse : System.Web.UI.Page
{
    public System.Text.StringBuilder builder = new System.Text.StringBuilder( "" );

    protected void Page_Load( object sender, EventArgs e )
    {

       
        string _act = Utils.GetRequest( "act", "all", 1, "", "" );

        //选择运行版本(酷币版Or金币版)
        int _sseType = Utils.ParseInt( Utils.GetRequest( "sseVe", "all", 1, "", "-1" ) );
        sseMgrBase _verMgr = null;
        switch( ( ESseVersionType ) _sseType )
        {
            case ESseVersionType.sseKB:
                _verMgr = new sseKbMgr();
                break;
            case ESseVersionType.sseJB:
                _verMgr = new sseJbMgr();
                break;
            default:
                Utils.Error( "版本类型不匹配", "" );
                break;
        }
        
        SSEManagePage.SseManagePageBase _pageObj = null;

        switch( _act )
        {
            case "state":
                _pageObj = new SSEManagePage.SSEPageState( this, this.Master,_verMgr );         //无需分版
                break;
            case "order":
                _pageObj = new SSEManagePage.SSEPageOrder( this, this.Master, _verMgr );        //分版完成
                break;
            case "charts":
                _pageObj = new SSEManagePage.SSEPageCharts( this, this.Master, _verMgr );        //分版完成
                break;
            case "prizepool":
                _pageObj = new SSEManagePage.SSEPagePrizepool( this, this.Master, _verMgr );      //分版完成
                break;
            case "analyse":
                _pageObj = new SSEManagePage.SSEPageAnalyse( this, this.Master, _verMgr );
                break;
            case "setting":
                _pageObj = new SSEManagePage.SSEPageSetting( this, this.Master, _verMgr );          //无需分版
                break;
            case "cancelOrderSubmit":
                _pageObj = new SSEManagePage.SSEPageCancelOrderSubmit( this, this.Master, _verMgr );   //分版完成
                break;
            case "reset":
                _pageObj = new SSEManagePage.SSEPageGameReset( this, this.Master, _verMgr );           //无需分版
                break;
            case "orderQuery":
                _pageObj = new SSEManagePage.SSEPageOrderQuery( this, this.Master, _verMgr );         //分版完成
                break;
            case "getPirzeConfirm":
                _pageObj = new SSEManagePage.SSEPageGetPirzeConfirm( this, this.Master, _verMgr );     //分版完成
                break;
            case "getPirzeSubMit":
                _pageObj = new SSEManagePage.SSEPageGetPirzeSubmit( this, this.Master, _verMgr );      //分版完成
                break;
            default:
                _pageObj = new SSEManagePage.SSEPageMain( this, this.Master, _verMgr );          //正在分版
                break;
        }
        _pageObj.ShowPage();
    }
}

namespace SSEManagePage
{
    public abstract class SseManagePageBase
    {
        protected Manage_game_sse mainPage;
        protected BCW.Common.BaseMaster baseMaster;
        protected string xmlPath = "/Controls/SSE.xml";
        protected int meid;
        protected int pageSize;
        private string pageTitle;

        protected sseMgrBase mSseVersionMgr;


        public SseManagePageBase( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
        {
            this.mainPage = _mainPage;
            this.baseMaster = _baseMaster;
            this.mSseVersionMgr = _sseMgr;
            meid = new BCW.User.Users().GetUsId();
            pageSize = int.Parse( ub.GetSub( "SSEManagePageSize", xmlPath ) );
            this.pageTitle = "<a href=\"" + Utils.getUrl( "default.aspx" ) + "\">游戏管理</a>&gt;<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=main" ) + "\">上证指数【" + this.mSseVersionMgr.account.name + "版】></a>";
        }

        public void SetTitle(string _title)
        {
            this.mainPage.builder.Append( Out.Tab( "<div  class=\"title\">", "" ) );
            this.mainPage.builder.Append( this.pageTitle + _title + "<br />" );  
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
        }



        public virtual void ShowPage()
        {
            
            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster,this.mSseVersionMgr ) );  
        }

         public void IncludePage(SseManagePageBase _page)
        {
            if (_page != null)
                _page.ShowPage();

        }

    }


    //页头
    public class SSEPageHeader :  SseManagePageBase
    {
        public SSEPageHeader( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void  ShowPage()
        {

            this.baseMaster.Title = ub.GetSub( "SSEName", xmlPath );

            this.mainPage.builder.Append( Out.Tab( "<div  class=\"text\">", "" ) );

            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=setting" ) + "\">游戏配置 </a><br />" );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=charts" ) + "\">游戏排行</a><br /> " );   
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=analyse" ) + "\">盈利分析 </a><br /> " );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=orderQuery" ) + "\">游戏查询 </a><br /> " );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool" ) + "\">奖池记录</a> <br /> " );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=state" ) + "\">游戏维护</a><br />" );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=reset" ) + "\">重置游戏</a><br />" );
            this.mainPage.builder.Append( "--------------------------------------------------<br />" );

        }          
    }

    //页脚
    public class SSEPageFooter : SseManagePageBase
    {
        public SSEPageFooter( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
              
            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
            this.mainPage.builder.Append( "<br />--------------------------------------------------<br />" );              
            this.mainPage.builder.Append( "<a href=\"" + Utils.getPage( this.mSseVersionMgr.pageName ) + "\">返回上一级</a>" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
            this.mainPage.builder.Append( Out.Tab( "<div class=\"title\">", "" ) );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( "../default.aspx" ) + "\">返回管理中心</a>" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

        }
    }

    //主页
    public class SSEPageMain : SseManagePageBase
    {
        private int pSseNo;


        public SSEPageMain( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        

        public override void ShowPage()
        {
            this.SetTitle( "投注" );

            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster,this.mSseVersionMgr) );

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
           
            int _thisSseNo = BCW.SSE.SSE.Instance().GetBuySseNo();


            pSseNo = Utils.ParseInt( Utils.GetRequest( "pSseNo", "get", 1, "", "" ) );
            if( pSseNo > 0 )
            {
                SetSseOpenTime( pSseNo ); 
                return;
            }

            string ac = Utils.GetRequest( "ac", "all", 1, "", "" );
            if( Utils.ToSChinese( ac ) == "确定修改" )
            {
                SaveSseOpenTime();
                return;
            }

            int pEditOpenPrize = Utils.ParseInt(Utils.GetRequest( "editOpenPrize", "get", 1, "", "" ));
            if( pEditOpenPrize > 0 )
            {
                EditOpenPrize(pEditOpenPrize);
                return;
            }
           
            ac = Utils.GetRequest( "ac", "all", 1, "", "" );
            if( Utils.ToSChinese( ac ) == "确定开奖" )
            {
                OpenPrizeComfirm( );
                return;
            }

                   

            //string pOpenTime =  Utils.GetRequest( "pOpenTime", "post", 1, "", "" );
            //string pCloseTime = Utils.GetRequest( "pCloseTime", "post", 1, "", "" ) ;
            //string pOpenPrizeTime = Utils.GetRequest( "pOpenPrizeTime", "post", 1, "", "" ) ;
            //if( string.IsNullOrEmpty( pOpenTime ) == false )
            //{
  
            //}


            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe","act", "sseNo", "backurl" };
            string strWhere = "";
            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;

            //往期
            // 开始读取列表
            IList<BCW.SSE.Model.SseBase> lstSseBase = new BCW.SSE.BLL.SseBase().GetSseBasePages( pageIndex, pageSize, strWhere, out recordCount );
            if( lstSseBase.Count > 0 )
            {
                //查询本期
                if( pageIndex == 1 )
                {
                    this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=main&amp;pSseNo=" + _thisSseNo ) + "\">【管理】></a>第{0}期开出指数：<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;pSseNo=" + _thisSseNo ) + "\">未开</a> <a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=main&amp;editOpenPrize="+_thisSseNo ) + "\">开奖</a> <br />", _thisSseNo ) );


                }

                int k = 1;
                foreach( BCW.SSE.Model.SseBase n in lstSseBase )
                {
                    if( k % 2 == 0 )
                        this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    else
                    {
                        if( k == 1 )
                            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                        else
                            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    }

                    this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=main&amp;pSseNo=" + n.sseNo ) + "\">【管理】></a>第{0}期开出指数：<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;pSseNo=" + n.sseNo ) + "\">{1}</a>【{2}】", n.sseNo, n.closePrice.ToString(), n.bz.Trim() == "1" ? "涨" : n.bz.Trim() == "0" ? "跌" : "平" ) );
                   // this.mainPage.builder.Append( string.Format( "第{0}期：{1}点【{2}】", n.sseNo,  ) );
                    k++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }


            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            base.ShowPage();
        }


        //设置上证开盘时间
        private void SetSseOpenTime(int pSseNo)
        {
            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

            //如果该期已经开奖，则不允许修改
            DataSet ds = new BCW.SSE.BLL.SseBase().GetList( "sseNo="+pSseNo );
            if( ds.Tables[ 0 ].Rows.Count > 0 )
            {
                Utils.Error( "该期已经开奖，不能设定相关参数", Utils.getUrl(this.mSseVersionMgr.pageName) );
                return;
            }
              

            //读取配置项.
            string _beginTime = ub.GetSub( "SSEBegin", xmlPath );
            string _beginEnd = ub.GetSub( "SSEEnd", xmlPath );
            string _beginOpen = ub.GetSub( "SSEOpen", xmlPath );

            //显示配置项
            string strText = "开盘时间:/,截止投注时间:/,开奖时间:(期号依据，若变更日期将会修改该期已下注的期号，请谨慎操作！)/,backurl";
            string strName = "pOpenTime,pCloseTime,pOpenPrizeTime,backurl";
            string strType = "text,text,text,hidden";
            string strValu = "" + _beginTime + "'" + _beginEnd + "'" + _beginOpen + "'" + Utils.getPage( 0 ) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "/确定修改,"+this.mSseVersionMgr.pageName+"act=main,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );

            this.mainPage.builder.Append( "<br /><a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=main" ) + "\">再看看吧</a> " );
            
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            base.ShowPage(); 
        }

        //保存开盘时间设置.
        private void SaveSseOpenTime()
        {


            string pOpenTime = Utils.GetRequest( "pOpenTime", "post", 1, "", "" );
            string pCloseTime = Utils.GetRequest( "pCloseTime", "post", 1, "", "" );
            string pOpenPrizeTime = Utils.GetRequest( "pOpenPrizeTime", "post", 1, "", "" );

            //日期校验.
            if( pOpenTime != "" && Regex.IsMatch( pOpenTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2} \d{2}:\d{2}:\d{2}$" ) == false )
                Utils.Error( "开盘时间填写格式有误", Utils.getUrl(this.mSseVersionMgr.pageName) );

            if( pCloseTime != "" && Regex.IsMatch( pCloseTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2} \d{2}:\d{2}:\d{2}$" ) == false )
                Utils.Error( "截止投注时间填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            if( pOpenPrizeTime != "" && Regex.IsMatch( pOpenPrizeTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2} \d{2}:\d{2}:\d{2}$" ) == false )
                Utils.Error( "开奖日期填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );


            //if( DateTime.Parse( pOpenTime ) <= DateTime.Now )
            //    Utils.Error( "开盘时间不能早于当前时间", "" );

            if( DateTime.Parse( pCloseTime ) <= DateTime.Parse( pOpenTime ) )
                Utils.Error( "截止投注时间不能早于开盘时间", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            if( DateTime.Parse( pOpenPrizeTime ).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse( pOpenPrizeTime ).DayOfWeek == DayOfWeek.Sunday || DateTime.Parse( pOpenPrizeTime ) <= DateTime.Parse( pCloseTime ) )
                Utils.Error( "开奖日期不能设定在周末 ，且不能早于截止投注时间", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            //修改开奖期号，变更相应数据期号
            string oldOpenPrizeTime = ub.GetSub( "SSEOpen", xmlPath );
            if( DateTime.Parse( oldOpenPrizeTime ).ToString( "yyyyMMdd" ) != DateTime.Parse( pOpenPrizeTime ).ToString( "yyyyMMdd" ) )
            {
                //变更相关数据期号.               
                IDataParameter[] _parameters = new SqlParameter[ 2 ];
                _parameters[ 0 ] = new SqlParameter( "@oldSseNo", SqlDbType.Int );
                _parameters[ 1 ] = new SqlParameter( "@newSseNo", SqlDbType.Int );
                _parameters[ 0 ].Value = DateTime.Parse( oldOpenPrizeTime ).ToString( "yyyyMMdd" );            //旧期号
                _parameters[ 1 ].Value = DateTime.Parse( pOpenPrizeTime ).ToString( "yyyyMMdd" );              //新期号   
    
                BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_sseUpdateSseNo", _parameters );  
            }

            //保存数据
            ub xml = new ub();
            this.mainPage.Application.Remove( xmlPath );//清缓存
            xml.ReloadSub( xmlPath );                   //加载配置  

            xml.dss[ "SSEBegin" ] = pOpenTime.ToString();
            xml.dss[ "SSEEnd" ] = pCloseTime.ToString();
            xml.dss[ "SSEOpen" ] = pOpenPrizeTime.ToString();

            System.IO.File.WriteAllText( this.mainPage.Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 );
            Utils.Success( "游戏状态管理", "设置成功，正在返回..", Utils.getUrl( this.mSseVersionMgr.pageName ), "1" );

        }

        //手工开奖编辑界面
        private void EditOpenPrize( int _editOpenPrize)
        {
            this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );

            this.mainPage.builder.Append( string.Format( "第{0}期", _editOpenPrize ) );
            this.mainPage.builder.Append( string.Format( "<br /><b style=\"color:#ff0000\">提示:开奖须是小数点两位，如：3205.33</b>", _editOpenPrize ) );

            if( System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday || System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday )
            {
                this.mainPage.builder.Append( "周末不能开奖");
            }


            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            //显示配置项
            string strText = "本期开出指数：,df,/,backurl";
            string strName = "openPrizeVal,tips,sseNo,backurl";
            string strType = "text,hidden,hidden,hidden";
            string strValu = "" + "''"+BCW.SSE.SSE.Instance().GetBuySseNo()+"'" + Utils.getPage( 0 ) + "";
            string strEmpt = "false,true,false,false";
            string strIdea = "/";
            string strOthe = "/确定开奖,"+this.mSseVersionMgr.pageName+"act=main,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );


            base.ShowPage();
        }

        //执行手工开奖
        private void OpenPrizeComfirm()
        {
            this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );

            
            string openPrizeVal = Utils.GetRequest( "openPrizeVal", "post", 1, "", "" );
            string sseNo = Utils.GetRequest( "sseNo", "post", 1, "", "" );


            //期数校验.
            if( sseNo != "" && Regex.IsMatch( sseNo, @"^\d*$" ) == false )
                Utils.Error( "开奖期数参数有误，无法开奖", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            //开奖校验.
            if( openPrizeVal != "" && Regex.IsMatch( openPrizeVal, @"^\d+\.\d{2,}$" ) == false )
                Utils.Error( "开奖指数至少要带两位小数(如123.45)", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            //周末不能开奖              
            if( System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday || System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday )
                Utils.Error( "周末不能开奖", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            //同一日不能多次开奖
            string _dayNo = System.DateTime.Now.Year.ToString( "#0000" ) + System.DateTime.Now.Month.ToString( "#00" ) + System.DateTime.Now.Day.ToString( "#00" );
            DataSet _ds = new BCW.SSE.BLL.SseBase().GetList( " sseNo=" + _dayNo );
            if( _ds.Tables[0].Rows.Count >0 )
                Utils.Error( "同一天不能多次开奖", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            //获取新一期的开奖数据.
            BCW.SSE.SSEDataParseNew _c = new BCW.SSE.SSEDataParseNew();
            _c.autoOpenPrize = false;
            _c.closePrice = openPrizeVal;
            if (_c.GetNewSSEData()== true)
              SetNextSseTime();
                                                  
            
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            Utils.Success( "开奖成功", "开奖成功，正在返回..", Utils.getUrl( this.mSseVersionMgr.pageName ), "1" );

            base.ShowPage();
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
            this.mainPage.Application.Remove( xmlPath );//清缓存
            xml.ReloadSub( xmlPath );                   //加载配置  

            xml.dss[ "SSEBegin" ] = _openTime.ToString("yyyy-MM-dd HH:mm:ss");
            xml.dss[ "SSEEnd" ] = _closeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
            xml.dss[ "SSEOpen" ] = _openPrizeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
            System.IO.File.WriteAllText( this.mainPage.Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 );
        }
    }

    //状态管理
    public class SSEPageState : SseManagePageBase
    {
        public SSEPageState( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster ,_sseMgr)
        {

        }

        public override void ShowPage()
        {


            //IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster ) );

            this.baseMaster.Title = "游戏维护";
            this.SetTitle( "游戏维护" );

            string ac = Utils.GetRequest( "ac", "all", 1, "", "" );

            ub xml = new ub();
            this.mainPage.Application.Remove( xmlPath );//清缓存
            xml.ReloadSub( xmlPath );                   //加载配置


            if( Utils.ToSChinese( ac ) == "确定修改" )
            {
                string SSEState = Utils.GetRequest( "SSEState", "post", 2, @"^[0-9]\d*$", "系统状态选择出错" );
                int robotState = Utils.ParseInt(Utils.GetRequest( "robotState", "post", 0, "^[0-1]$", "机器人开关格式错误" ));
                string robotId = Utils.GetRequest( "robotId", "post", 1, "", "" );
                string robotBPrice =  Utils.GetRequest( "robotBPrice", "post", 1, "", "" ) ;
                int robotOrderSpace = Utils.ParseInt(Utils.GetRequest( "robotOrderSpace", "post", 0, "^[0-9]/d*$", "机器人投注间隔格式错误" ));
                string SSEDemoIDS = Utils.GetRequest( "SSEDemoIDS", "post", 1, "", "" );

                if( Regex.IsMatch( robotBPrice, @"^(\d+\#)+\d+$" ) == false )
                    Utils.Error( "机器人投注金额设置格式有错，用#分隔(最少填两个数值)", Utils.getUrl( this.mSseVersionMgr.pageName ) );

                xml.dss[ "SSEState" ] = SSEState;       
                xml.dss[ "SSERobotState" ] = robotState;
                xml.dss[ "SSERobot" ] = robotId;
                xml.dss[ "SSERobotBPrice" ] = robotBPrice;
                xml.dss[ "SSERobotOrderSpace" ] = robotOrderSpace;
                xml.dss[ "SSEDemoIDS" ] = SSEDemoIDS;
                                                   

                System.IO.File.WriteAllText( this.mainPage.Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 );
                Utils.Success( "游戏维护", "设置成功，正在返回..", Utils.getUrl( this.mSseVersionMgr.pageName+"act=state" ), "1" );   

                return;
            }

            this.mainPage.builder.Append( "<br />-------------------------------------------" );


            //显示配置项
            string strText = "〓游戏状态〓/,/〓机器人配置〓/,/机器人ID/,/随机投注额(最少2个，用#号分隔)/,/投注间隔(秒):/,/〓内测号配置〓/,backurl";
            string strName = "SSEState,robotState,robotId,robotBPrice,robotOrderSpace,SSEDemoIDS,backurl";
            string strType = "select,select,textarea,text,num,textarea,hidden";
            string strValu = "" + xml.dss[ "SSEState" ] + "'" + xml.dss[ "SSERobotState" ] + "'" + xml.dss[ "SSERobot" ] + "'" + xml.dss[ "SSERobotBPrice" ] + "'" + xml.dss[ "SSERobotOrderSpace" ] + "'" + xml.dss[ "SSEDemoIDS" ] + "'"  +Utils.getPage( 0 ) + "";
            string strEmpt = "0|关服|1|内测|2|开服,0|关|1|开,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,"+this.mSseVersionMgr.pageName+"act=state,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );

            //this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );
            //string[] _arryRobot = xml.dss[ "SSERobot" ].ToString().Split( '#' );
            //this.mainPage.builder.Append( "<br />========以下是已设置机器人ID=========<br />" );
            //foreach( string _robotId in _arryRobot )
            //{
                
            //    this.mainPage.builder.Append( _robotId + "<br />" );
            //}


            //string[] _arryText = xml.dss[ "SSEDemoIDS" ].ToString().Split( '#' );
            //this.mainPage.builder.Append( "<br />========以下是已设置内测号ID=========<br />" );
            //foreach( string _textId in _arryText )
            //{
            //    this.mainPage.builder.Append( _textId + "<br />" );
            //}  
            base.ShowPage();
        }
    }

    //投注管理
    public class SSEPageOrder : SseManagePageBase
    {
        private int ptype;          //请求类型
        private string useDateTime;

        public SSEPageOrder( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        private void ShowQueryDateTime()
        {
            string strText = "订单ID(标识),玩家ID：,启用时间段查询：,开始日期：,结束日期：";
            string strName = "orderId,userId,useDateTime,sTime,oTime";
            string strType = "num,num,select,date,date";
            string strValu = "''"+int.Parse(useDateTime)+"'" + DT.FormatDate( DateTime.Now.AddMonths( -1 ), 0 ) + "'" + DT.FormatDate( DateTime.Now, 0 ) + "";
            string strEmpt = "false,false,0|不启用|1|启用,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,"+this.mSseVersionMgr.pageName+"act=order,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );
        }

        public override void ShowPage()
        {
           // IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster ) );

            this.baseMaster.Title = "投注查询";
            this.SetTitle( "投注查询" );    

            //是否退注
            int _cancelOrderId = Utils.ParseInt( Utils.GetRequest( "cancelOrderId", "get", 1, @"^\d*", "无效的订单标识ID" ) );
            if( _cancelOrderId > 0 )
            {
                cancelOrder( _cancelOrderId );
                base.ShowPage();
                return;
            }


            this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );

            string pSseNo = Utils.GetRequest( "pSseNo", "get", 1, "", "" );

           /* if( pSseNo != "" )
            {
                this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;ptype=0" + "&amp;pSseNo=" + pSseNo ) + "\">全部</a> " );
                this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;ptype=1" + "&amp;pSseNo=" + pSseNo ) + "\">未开奖</a> " );
                this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;ptype=2" + "&amp;pSseNo=" + pSseNo ) + "\">已开奖</a> " );
                this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;ptype=3" + "&amp;pSseNo=" + pSseNo ) + "\">未兑奖</a> " );
                this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;ptype=4" + "&amp;pSseNo=" + pSseNo ) + "\">已兑奖</a> <br />" );
            }
             */
                          
            ptype = Utils.ParseInt( Utils.GetRequest( "ptype", "get", 1, @"^[0-4]$", "0" ) );
            string orderId =  Utils.GetRequest( "orderId", "post", 1, "", "" ).Trim();
            string userId = Utils.GetRequest( "userId", "post", 1, "", "" ).Trim();
            useDateTime =  Utils.GetRequest( "useDateTime", "post", 1, @"^[0-1]$", "0" );
            string beginTime = Utils.GetRequest( "sTime", "post", 1, "", "" );
            string endTime = Utils.GetRequest( "oTime", "post", 1, "", "" );


            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe", "act", "ptype", "pSseNo", "backurl" };

            string strWhere = " 1=1 and orderType="+this.mSseVersionMgr.versionId;// string.Format( "userId ={0}", meid );


            
            if  (string.IsNullOrEmpty( orderId ) == false)
            {
                if (Regex.IsMatch( orderId, @"^[0-9]*$" ) == false )
                    Utils.Error( "订单标识填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                else              
                    strWhere += " and id=" + orderId;
            }

            if( string.IsNullOrEmpty( userId ) == false) 
            {
               if (Regex.IsMatch( userId, @"^[0-9]*$" ) == false )
                   Utils.Error( "订单标识填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                else
                    strWhere += " and userId=" + userId;
            }

            if( string.IsNullOrEmpty( pSseNo ) == false )
            {
                if( Regex.IsMatch( pSseNo, @"^\d*$" ) == false )
                    Utils.Error( "期数格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                else
                    strWhere += " and sseNo=" + pSseNo;
            }


            if( int.Parse(useDateTime) == 1 )
            {
                if( beginTime != "" && Regex.IsMatch( beginTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2} [0-9]{2}:[0-9]{2}:[0-9]{2}$" ) == false )
                    Utils.Error( "开始日期填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                else
                    strWhere += " and orderDateTime>='" + beginTime+"'";


                if( endTime != "" && Regex.IsMatch( endTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2} [0-9]{2}:[0-9]{2}:[0-9]{2}$" ) == false )
                    Utils.Error( "结束日期填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                else
                    strWhere += " and orderDateTime<='" + endTime+"'";             
            }


            switch (ptype)
            {
                case 1:
                    this.mainPage.Master.Title = "投注管理-查询未开奖";
                    strWhere = strWhere.Replace("1=1" ," sseNo=" + BCW.SSE.SSE.Instance().GetBuySseNo());        //查询未开奖
                    break;
                case 2:
                    this.mainPage.Master.Title = "投注管理-查询已开奖";
                    strWhere = strWhere.Replace( "1=1", " sseNo!=" + BCW.SSE.SSE.Instance().GetBuySseNo() );        //查询已开奖
                    break;
                case 3:
                    this.mainPage.Master.Title = "投注管理-查询未兑奖";
                    strWhere = strWhere.Replace( "1=1", " sseNo!=" + BCW.SSE.SSE.Instance().GetBuySseNo() + " and oState='未兑奖'" );        //查询已开奖未兑奖
                    break;
                case 4:
                    this.mainPage.Master.Title = "投注管理-查询已兑奖";
                    strWhere = strWhere.Replace( "1=1", " sseNo!=" + BCW.SSE.SSE.Instance().GetBuySseNo() + " and oState='已兑奖'" );        //查询已开奖未兑奖
                    break; 
                default :
                    this.mainPage.Master.Title = "投注管理-查询所有投注记录";
                    break;

            }  

            string ac = Utils.GetRequest( "ac", "all", 1, "", "" );
            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;


            // 开始读取列表 
            IList<SseManageClass.ManagerOrder> lstSseOrder = BCW.SSE.SSEManage.Instance().GetSseOrderPages( pageIndex, pageSize, strWhere, out recordCount );
            if( lstSseOrder.Count > 0 )
            {
                int k = 1;
                if( string.IsNullOrEmpty( pSseNo ) == false )
                {
                    DataSet _ds = new BCW.SSE.BLL.SseBase().GetList( "sseNo=" + pSseNo );
                    if( _ds.Tables[ 0 ].Rows.Count > 0 )
                        this.mainPage.builder.Append( string.Format( "<b style=\"color:#ff0000\">〓第{0}期〓开奖：{1}【{2}】<br /></b>", pSseNo, _ds.Tables[ 0 ].Rows[ 0 ][ "closePrice" ], _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "1" ? "涨" : _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "0" ? "跌" : "平" ) );
                    else
                        this.mainPage.builder.Append( string.Format( "<b style=\"color:#ff0000\">〓第{0}期〓未开奖<br /></b>", pSseNo ) );
                }
                foreach( SseManageClass.ManagerOrder n in lstSseOrder )
                {
                    if( k % 2 == 0 )
                        this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    else
                    {
                        if( k == 1 )
                            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                        else
                            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    }

                    //this.mainPage.builder.Append( string.Format( "【{0}】,{1},{2}",n.id.ToString(),n.buyType.ToString(),n.sseNo.ToString()));
                    this.mainPage.builder.Append( string.Format( "{0}、{1}|标识({2})|{3}|投{4}|", k+((pageIndex-1)*pageSize) , "<a href=\"" + Utils.getUrl( "/bbs/uinfo.aspx?uid=" + n.userId + "&amp;backurl=" + Utils.PostPage( 1 ) + "" ) + "\">" + BCW.User.Users.SetUser( n.userId ) + "("+n.userId+")</a>" , n.id.ToString(), n.buyType ? "猜涨" : "猜跌", n.buyMoney.ToString( "#0" )+this.mSseVersionMgr.account.name ) );
                    if( n.prizeVal > 0 )
                    {
                        this.mainPage.builder.Append( string.Format( "<b style=\"color:#ff0000\">赢{0}</b>", n.prizeVal.ToString( "#0" ) + this.mSseVersionMgr.account.name ) );    
                    } 
                    this.mainPage.builder.Append( string.Format( "【{0}】", n.sseNo == BCW.SSE.SSE.Instance().GetBuySseNo() ? "未开奖" : n.state ) );
                    this.mainPage.builder.Append( string.Format( "({0})", n.orderDateTime ) );
                    if( n.sseNo == BCW.SSE.SSE.Instance().GetBuySseNo() )
                    {
                        this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;cancelOrderId=" + n.id ) + "\">【退注】</a>" );
                    }

                    if( n.state == "未兑奖" )
                    {      
                        DataSet _ds = new BCW.SSE.BLL.SseGetPrize().GetList( 1, "orderId=" + n.id, "id" );
                        if (_ds.Tables[0].Rows.Count >0)
                            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=getPirzeConfirm&amp;prizeId=" + _ds.Tables[ 0 ].Rows[ 0 ][ "id" ].ToString() ) + "\">【帮他兑奖】</a>" );
                    }

                    k++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }     

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }

            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );

           ShowQueryDateTime();

                     


            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );                  


           
            base.ShowPage();
        }


        //退注处理
        private void cancelOrder( int _orderId )
        {
            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );

            BCW.SSE.Model.SseOrder _objModel = new BCW.SSE.BLL.SseOrder().GetModel( _orderId );
            if( _objModel != null )
            {
                this.mainPage.builder.Append( string.Format( "==请您确认以下退注信息==:<br />" ) );
                this.mainPage.builder.Append( string.Format( "订单标识:{0}<br />", _orderId) );
                this.mainPage.builder.Append( string.Format( "申请帐户:<a href=\"/bbs/uinfo.aspx?uid=" + _objModel.userId + "\">{0}({1})</a><br />", new BCW.BLL.User().GetUsName( _objModel.userId ) + "(" + _objModel.userId + ")", _objModel.userId ) );
                this.mainPage.builder.Append( string.Format( "期数：第{0}期<br />", _objModel.sseNo ) );
                this.mainPage.builder.Append( string.Format( "竞猜类型:{0}<br />", _objModel.buyType == true ? "猜涨" : "猜跌" ) );
                this.mainPage.builder.Append( string.Format( "退注金额:{0}<br />", _objModel.buyMoney.ToString("#0")+this.mSseVersionMgr.account.name ) );
            }

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            string strText = ",,";
            string strName = "sseNo,orderId,userid";
            string strType = "hidden,hidden,hidden";
            string strValu = ""+_objModel.sseNo+"'" + _orderId + "'" + _objModel .userId+ "";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "确认退注,"+this.mSseVersionMgr.pageName+"act=cancelOrderSubmit,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );


            
        }
    }



   //确认退注
    public class SSEPageCancelOrderSubmit : SseManagePageBase
    {
        public SSEPageCancelOrderSubmit( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            //注册手工注册防刷
            int Expir = Utils.ParseInt( ub.GetSub( "SSEBuyExpir", xmlPath ) );
            string CacheKey = Utils.GetUsIP() + "_sse";
            object getObjCacheTime = DataCache.GetCache( CacheKey );
            if( getObjCacheTime != null )
            {
                Utils.Error( ub.GetSub( "BbsGreet", "/Controls/bbs.xml" ), Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }
            object ObjCacheTime = DateTime.Now;
            DataCache.SetCache( CacheKey, ObjCacheTime, DateTime.Now.AddSeconds( Expir ), TimeSpan.Zero );

             string orderId =  Utils.GetRequest( "orderId", "post", 1, "", "" ).Trim();
             string userId = Utils.GetRequest( "userid", "post", 1, "", "" ).Trim();
             string sseNo = Utils.GetRequest( "sseNo", "post", 1, "", "" ).Trim(); 

            //更改订单状态.
             BCW.SSE.Model.SseOrder modelSseOrder = new BCW.SSE.BLL.SseOrder().GetModel( int.Parse(orderId ));
             this.mainPage.builder.Append("接收订单号:"+orderId);

             if( modelSseOrder == null )
             {
                 Utils.Success( "确认退注", "找不到对应的订单号", Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;pSseNo="+modelSseOrder.sseNo ), "1" );
                 return;
             }
             modelSseOrder.state = 1;
             new BCW.SSE.BLL.SseOrder().Update( modelSseOrder );

            //变更奖池.                  
             IDataParameter[] _parameters = new SqlParameter[ 3 ];
             _parameters[ 0 ] = new SqlParameter( "@orderId", SqlDbType.Int );
             _parameters[ 1 ] = new SqlParameter( "@operType", SqlDbType.Int );
             _parameters[ 2 ] = new SqlParameter( "@operUserId", SqlDbType.Int );
             _parameters[ 0 ].Value = orderId;
             _parameters[ 1 ].Value = 2;             //退注.   
             _parameters[ 2 ].Value = userId;
             BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseChangePrizePool", _parameters );


             //返还币
             this.mSseVersionMgr.account.UpdateiGold(modelSseOrder.userId, ( Int64 ) modelSseOrder.buyMoney, string.Format( "上证|{0}期|申请退回|标识({1})", sseNo, orderId ) );

             //new BCW.BLL.User().UpdateiGold( modelSseOrder.userId, ( Int64 ) modelSseOrder.buyMoney, string.Format( "上证|{0}期|申请退回|标识({1})", sseNo, orderId ) );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
            Utils.Success( "确认退注", "退注成功", Utils.getUrl( this.mSseVersionMgr.pageName+"act=order&amp;pSseNo="+modelSseOrder.sseNo ), "1" );

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
            base.ShowPage();
        }

    }


    //排行榜
    public class SSEPageCharts : SseManagePageBase
    {
        private int ptype;

        public SSEPageCharts( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.baseMaster.Title = "游戏排行";
            this.SetTitle( "游戏排行" );

            ptype = Utils.ParseInt( Utils.GetRequest( "ptype", "get", 1, @"^[0-3]$", "0" ) );
            if( ptype < 0 )
                ptype = 0;

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

            this.mainPage.builder.Append( ptype == 0 ?"日榜 ":"<a href=\"" + Utils.getUrl(this.mSseVersionMgr.pageName+"act=charts&amp;ptype=0" ) + "\">日榜</a> " );
            this.mainPage.builder.Append( ptype == 1 ? "周榜 " : "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=charts&amp;ptype=1" ) + "\">周榜</a> " );
            this.mainPage.builder.Append( ptype == 2 ? "月榜 " : "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=charts&amp;ptype=2" ) + "\">月榜</a> " );
            this.mainPage.builder.Append( ptype == 3 ? "总榜 " : "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=charts&amp;ptype=3" ) + "\">总榜</a> " );

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            string pquery = Utils.GetRequest( "query", "post", 1, "", "" );
            string beginTime = Utils.GetRequest( "sTime", "post", 1, "", "" );
            string endTime = Utils.GetRequest( "oTime", "post", 1, "", "" );

            int pageIndex;
            int recordCount;
            int pageSize = 10;
            string strWhere = "1=1";
            string[] pageValUrl = { "sseVe", "act", "backurl", "ptype", "sTime", "oTime" };
            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;
            if( pageIndex > 10 )
                pageIndex = 10;

            switch( ptype )
            {
                case 0: //日榜
                    strWhere = "CONVERT(varchar,p.openDateTime,112) in ( select top 1 CONVERT(varchar,openDateTime,112) from  tb_SseGetPrize order by id desc) and p.orderType=" + this.mSseVersionMgr.versionId;
                    break;
                case 1: //周榜
                    strWhere = "CONVERT(varchar,p.openDateTime,112) >= CONVERT(VARCHAR,DATEADD(DAY,-7,GETDATE()),112) and p.orderType=" + this.mSseVersionMgr.versionId;
                    break;
                case 2: //月榜
                    strWhere = "CONVERT(varchar,p.openDateTime,112) >= CONVERT(VARCHAR,DATEADD(MONTH,-7,GETDATE()),112) and p.orderType=" + this.mSseVersionMgr.versionId;
                    break;
                case 3: //总榜
                    strWhere = "1=1 and p.orderType=" + this.mSseVersionMgr.versionId;
                    break;
            }


            if( pquery == "4" )
            {

                if( beginTime != "" && Regex.IsMatch( beginTime, @"^\d{4}-\d{1,2}-\d{1,2}$" ) == false )
                    Utils.Error( "开始日期格式填写有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );

                if( endTime != "" && Regex.IsMatch( endTime, @"^\d{4}-\d{1,2}-\d{1,2}$" ) == false )
                    Utils.Error( "结束日期格式填写有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                strWhere = string.Format( "CONVERT(varchar,openDateTime,112) >= '{0}' and CONVERT(varchar,openDateTime,112)<='{1}'", beginTime, endTime );
            }

            // 开始读取列表
            IList<BCW.SSE.Model.SseGetPrizeCharts> listSseGetPrizeCharts = new BCW.SSE.BLL.SseGetPrize().GetSseGetPrizeChartsPages( pageIndex, pageSize, strWhere, out recordCount );
            if( listSseGetPrizeCharts.Count > 0 )
            {
                int k = 1;
                foreach( BCW.SSE.Model.SseGetPrizeCharts n in listSseGetPrizeCharts )
                {
                    if( k % 2 == 0 )
                        this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    else
                    {
                        if( k == 1 )
                            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                        else
                            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    }
                    this.mainPage.builder.Append( "[第" + ( ( pageIndex - 1 ) * pageSize + k ) + "名]<a href=\"" + Utils.getUrl( "/bbs/uinfo.aspx?uid=" + n.userId + "&amp;backurl=" + Utils.PostPage( 1 ) + "" ) + "\">" + BCW.User.Users.SetUser( n.userId ) + "(" + n.userId + ")</a>" + "赢得" + n.prizeVal + "" + this.mSseVersionMgr.account.name + "" );
                    k++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }

                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );
            
            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录.." ) );
            }

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            string strText = "开始日期：,结束日期：,,";
            string strName = "sTime,oTime,query,backurl";
            string strType = "text,text,hidden,hidden";
            string strValu = "" + DateTime.Now.AddMonths( -1 ).ToString( "yyyy-MM-dd" ) + "'" + DateTime.Now.ToString( "yyyy-MM-dd" ) + "'" + "4'" + "";

            string strEmpt = "true,true,false,false";
            string strIdea = "/";
            string strOthe = "马上查询/,"+this.mSseVersionMgr.pageName+"act=charts" + ",post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );


            base.ShowPage();
        }
    }

    //奖池查询
    public class SSEPagePrizepool : SseManagePageBase
    {

        private int ptype;              //页面类型
        private int pageIndex;
        private int recordCount;
        private string[] pageValUrl = { "sseVe","act", "ptype", "pSseNo", "backurl" };  
        private string strWhere = " 1=1";

        
        private int sumPrizePoolVal;            //总奖池金额
        private int sumBuyUpVal;                //本期猜涨总金额
        private int sumBuyDownVal;              //本期猜跌总金额



        public SSEPagePrizepool( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.SetTitle( "奖池记录" );
            strWhere = "1=1 and p.prizeType=" + this.mSseVersionMgr.versionId;

            //IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster ) );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

           // this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=0" ) + "\">奖池概览</a> " );
           // this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=1" ) + "\">奖池明细</a> <br /> " );
                                                 
            ptype = Utils.ParseInt(Utils.GetRequest("ptype","get",1,@"^[0-1]$","请求类型错误"));

            this.baseMaster.Title = "奖池查询-" + (ptype == 0 ? "奖池概览" : "奖池明细");

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );


            string ac = Utils.GetRequest( "ac", "all", 1, "", "" );
            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;   


            switch( ptype )
            {
                case 0:
                    PoolOveriew();          //奖池概览
                    break;
                case 1:
                    PoolDetail();
                    //PoolDetail();         //奖池明细
                    break;
            }
               

            base.ShowPage();
        }

        //奖池概览
        private void PoolOveriew()
        {

            DateTime _time;
            string _sTime;


            string beginTime = Utils.GetRequest( "sTime", "post", 1, "", "" );
            string endTime = Utils.GetRequest( "oTime", "post", 1, "", "" );

            //日期校验.
            if( beginTime != "" && Regex.IsMatch( beginTime, @"^\d{4}\d{1,2}\d{1,2}$" ) == false )
                Utils.Error( "开始期数格式填写有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            if( endTime != "" && Regex.IsMatch( endTime, @"^\d{4}\d{1,2}\d{1,2}$" ) == false )
                Utils.Error( "结束期数格式填写有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );    



            if( string.IsNullOrEmpty( beginTime ) == false )
            {

                strWhere += " and p.sseNo>=" + beginTime;
            }


            if( string.IsNullOrEmpty( endTime ) == false )
            {
                strWhere += " and p.sseNo<=" + endTime;
            }
               

            // 开始读取列表 
            IList<SseManageClass.ManagerPoolOveriew> lstSseOrder = BCW.SSE.SSEManage.Instance().GetPoolOveriew( pageIndex, pageSize, strWhere, out recordCount );
            if( lstSseOrder.Count > 0 )
            {
                int k = 1;
                foreach( SseManageClass.ManagerPoolOveriew n in lstSseOrder )
                {
                    if( k % 2 == 0 )
                        this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    else
                    {
                        if( k == 1 )
                            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                        else
                            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    }


                    ////获取总奖池值
                    //decimal sumBeforeOpenPrizeVal = new BCW.SSE.BLL.SsePrizePoolChang().GetBeforeOpenPrizeVal( n.sseNo );
                    ////获取本期猜涨总额
                    //decimal sumUpVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( n.sseNo, 1 );
                    //decimal sumDownVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( n.sseNo, 0 );

                    //this.mainPage.builder.Append( string.Format( "【{0}】,{1},{2}",n.id.ToString(),n.buyType.ToString(),n.sseNo.ToString()));
                    if( n.sseNo != BCW.SSE.SSE.Instance().GetBuySseNo() )
                        this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">第【{0}】期奖池</a>【{1}】", n.sseNo.ToString(), n.openResult ) );
                    //this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">第【{0}】期奖池</a>【{1}】<b style=\"color:#ff0000\"> | 总奖池：{2} | 猜涨：{3} | 猜跌：{4}</b>", n.sseNo.ToString(), n.openResult, sumBeforeOpenPrizeVal.ToString( "#0" ), sumUpVal.ToString( "#0" ), sumDownVal.ToString( "#0" ) ) );
                    else
                        this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">系统当前奖池<b style=\"color:#ff0000\">(第{0}期)</b></a><b style=\"color:#ff0000\"> | 结余：{1} </b>", n.sseNo.ToString(), n.poolVal.ToString( "#0" )+ this.mSseVersionMgr.account.name) );
                    //this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">系统当前奖池<b style=\"color:#ff0000\">(第{0}期)</b></a><b style=\"color:#ff0000\"> | 结余：{1} | 猜涨：{2} | 猜跌：{3}</b>", n.sseNo.ToString(), n.poolVal.ToString( "#0.00" ), sumUpVal.ToString( "#0" ), sumDownVal.ToString( "#0" ) ) );
                    k++;
                   
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }

                this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                this.mainPage.builder.Append( "<br />" );
                this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }


            string strText = "开始期数：,结束期数：,";
            string strName = "sTime,oTime,backurl";
            string strType = "text,text,hidden";
            string strValu = "" + DateTime.Now.AddMonths( -1 ).ToString( "yyyyMMdd" ) + "'" + DateTime.Now.ToString( "yyyyMMdd" ) + "'" + "" + "";

            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "马上查询,"+this.mSseVersionMgr.pageName+"act=prizepool&amp;ptype=" + ptype + ",post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );     

        }

        //奖池明细
        private void PoolDetail()
        {
            DateTime _time;
            string _sTime;

            strWhere = "1=1 and poolType=" + this.mSseVersionMgr.versionId;

            string sseNo = Utils.GetRequest( "pSseNo", "get", 1, "", "" );

            if( string.IsNullOrEmpty( sseNo ) == false )
            {

                strWhere += " and sseNo=" + sseNo;
            }

            //if( string.IsNullOrEmpty( beginTime ) == false )
            //{

            //    _time = DateTime.Parse( beginTime );
            //    _sTime = _time.Year.ToString( "#0000" ) + _time.Month.ToString( "#00" ) + _time.Day.ToString( "#00" );

            //    strWhere += " and Convert(varchar,a.changeTime,112) >=" + _sTime;
            //}


            //if( string.IsNullOrEmpty( endTime ) == false )
            //{
            //    _time = DateTime.Parse( endTime );
            //    _sTime = _time.Year.ToString( "#0000" ) + _time.Month.ToString( "#00" ) + _time.Day.ToString( "#00" );

            //    strWhere += " and Convert(varchar,a.changeTime,112) <=" + _sTime;
            //}



            // 开始读取列表 
            IList<BCW.SSE.Model.SsePrizePoolChang> lstSsePrizePoolChang = new BCW.SSE.BLL.SsePrizePoolChang().GetSsePrizePoolChangePages( pageIndex, pageSize, strWhere, out recordCount );
            if( lstSsePrizePoolChang.Count > 0 )
            {
                int k = 1;
                //获取总奖池值
                decimal sumBeforeOpenPrizeVal = new BCW.SSE.BLL.SsePrizePoolChang().GetBeforeOpenPrizeVal( int.Parse( sseNo ) );
                //获取本期猜涨总额
                decimal sumUpVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney(this.mSseVersionMgr.versionId, int.Parse( sseNo ), 1 );
                decimal sumDownVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( this.mSseVersionMgr.versionId, int.Parse( sseNo ), 0 );

                this.mainPage.builder.Append( string.Format( "<b style=\"color:#ff0000\">第{0}期 | 总奖池：{1} | 猜涨：{2} | 猜跌：{3}</b><br />", sseNo, sumBeforeOpenPrizeVal.ToString( "#0" ) + this.mSseVersionMgr.account.name, sumUpVal.ToString( "#0" ) + this.mSseVersionMgr.account.name, sumDownVal.ToString( "#0" ) + this.mSseVersionMgr.account.name ) );
                        
                foreach( BCW.SSE.Model.SsePrizePoolChang n in lstSsePrizePoolChang )
                {
                    if( k % 2 == 0 )
                        this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    else
                    {
                        if( k == 1 )
                            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
                        else
                            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    }

                    string coinName = this.mSseVersionMgr.account.name; 
                    switch( n.operType )
                    {
                        //0下单/1系统退注(开奖为平)/2人工退注/3系统开平局退注/4本期滚存到下期/5下期得到本期滚存
                        case 0:
                            this.mainPage.builder.Append( string.Format( "{0}、<a href=\"/bbs/uinfo.aspx?uid=" + n.OperId + "\">{1}</a>在第{2}期<b style=\"color:#06A545\">消费{3}{4}</b>|<b style=\"color:#ff0000\">结余：{5}</b>({6})",k+((pageIndex-1)*pageSize), new BCW.BLL.User().GetUsName( n.OperId ) + "(" + n.OperId + ")", n.sseNo, n.changeMoney.ToString( "#0" ), coinName, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );
                            break;
                        case 1:
                            this.mainPage.builder.Append( string.Format( "{0}、系统在第{1}期开出平局<b style=\"color:#06A545\">退回投注{2}{3}</b>|<b style=\"color:#ff0000\">结余：{4}</b>", k + ( ( pageIndex - 1 ) * pageSize ), n.sseNo, n.changeMoney.ToString( "#0" ), coinName, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );                             
                            break;
                        case 2:
                            this.mainPage.builder.Append( string.Format( "{0}、<a href=\"/bbs/uinfo.aspx?uid=" + n.OperId + "\">{1}</a>在第{2}期<b style=\"color:#06A545\">申请退注{3}{4}</b>|<b style=\"color:#ff0000\">结余：{5}</b>({6})", k + ( ( pageIndex - 1 ) * pageSize ), new BCW.BLL.User().GetUsName( n.OperId ) + "(" + n.OperId + ")", n.sseNo, n.changeMoney.ToString( "#0" ), coinName, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );
                             break;
                        case 3:
                             this.mainPage.builder.Append( string.Format( "{0}、系统第{1}期<b style=\"color:#06A545\">派奖{2}{3}</b>|<b style=\"color:#ff0000\">结余：{4}{5}</b>({6})", k + ( ( pageIndex - 1 ) * pageSize ), n.sseNo, n.changeMoney.ToString( "#0" ), coinName, n.bz, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );             
                            break;
                        case 4:
                            this.mainPage.builder.Append( string.Format( "{0}、第{1}期<b style=\"color:#06A545\">滚存{2}{3}</b>到第{4}期|<b style=\"color:#ff0000\">结余：{5}</b>({6})", k + ( ( pageIndex - 1 ) * pageSize ), n.sseNo, n.totalMoney.ToString( "#0" ), coinName, n.bz, 0, n.changeTime ) );   
                            break;
                        case 5:
                            this.mainPage.builder.Append( string.Format( "{0}、第{1}期得到第{2}期<b style=\"color:#06A545\">滚存{3}{4}</b>|<b style=\"color:#ff0000\">结余：{5}</b>({6})", k + ( ( pageIndex - 1 ) * pageSize ), n.sseNo, n.bz, n.changeMoney.ToString( "#0" ), coinName, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );
                            break;
                    }


                    k++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }
                this.mainPage.builder.Append( "<br />" );

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount,Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }

        }
    }

    //数据分析
    public class SSEPageAnalyse : SseManagePageBase
    {
        private string beginTime;
        private string endTime;
        private int includeRobot;


        public SSEPageAnalyse( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.baseMaster.Title = "盈利分析";
            this.SetTitle( "盈利分析" ); 
            

            beginTime = Utils.GetRequest( "sTime", "post", 1, "", "" );
            endTime = Utils.GetRequest( "oTime", "post", 1, "", "" );
            includeRobot = Utils.ParseInt(Utils.GetRequest( "includeRobot", "post", 1, "", "" ));

            //日期校验.
            if( beginTime != "" && Regex.IsMatch( beginTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2}$" ) == false )
                Utils.Error( "开始日期填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );

            if( endTime != "" && Regex.IsMatch( endTime, @"^\d{4}(\-)\d{1,2}\-\d{1,2}$" ) == false )
                Utils.Error( "结束日期填写格式有误", Utils.getUrl( this.mSseVersionMgr.pageName ) );     

            if (string.IsNullOrEmpty(beginTime))
               beginTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(endTime))
               endTime = DateTime.Now.ToString("yyyy-MM-dd");


            this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );

              
            IDataParameter[] _parameters = new SqlParameter[4];
            _parameters[ 0 ] = new SqlParameter( "@includeRobot", SqlDbType.Bit );
            _parameters[ 1 ] = new SqlParameter( "@BDate", SqlDbType.DateTime );
            _parameters[ 2 ] = new SqlParameter( "@EDate", SqlDbType.DateTime );
            _parameters[ 3 ] = new SqlParameter( "@orderType", SqlDbType.Int );
            _parameters[ 0 ].Value = includeRobot;
            _parameters[ 1 ].Value = DateTime.Parse( beginTime );
            _parameters[ 2 ].Value = DateTime.Parse( endTime );
            _parameters[ 3 ].Value = this.mSseVersionMgr.versionId;
            SqlDataReader _reader = BCW.Data.SqlHelper.RunProcedure( "sp_SseCalcProfit", _parameters );
            while( _reader.Read() )
            {
                this.mainPage.builder.Append("【"+ _reader.GetString( 0 ) + "】<br />" );
                this.mainPage.builder.Append( string.Format( "总收入：{0} (销售收入：{1}+ 手续费收入：{2})<br />", _reader.GetDecimal( 1 ).ToString( "0" ) + this.mSseVersionMgr.account.name, _reader.GetDecimal( 2 ).ToString( "0" ) + this.mSseVersionMgr.account.name, _reader.GetDecimal( 3 ).ToString( "0" ) + this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "总支出：{0} <br />", _reader.GetDecimal( 4 ).ToString( "0" ) + this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "总盈利：{0} <br />", _reader.GetDecimal( 5 ).ToString( "0" ) + this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( "<br />" ); 
            }

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );


            string strText = "包含机器人,开始日期：,结束日期：,";
            string strName = "includeRobot,sTime,oTime,backurl";
            string strType = "select,text,text,hidden";
            string strValu = "" + includeRobot.ToString()+ "'" + beginTime + "'" + endTime + "'" + "" + "";

            string strEmpt = "0|不包含|1|包含,true,true,false";
            string strIdea = "/";
            string strOthe = "马上查询,"+this.mSseVersionMgr.pageName+"act=analyse,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );

            base.ShowPage();
        }
    }

    //配置中心
    public class SSEPageSetting : SseManagePageBase
    {
        public SSEPageSetting( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.baseMaster.Title = "游戏配置";
            this.SetTitle( "游戏配置" );             

            string ac = Utils.GetRequest( "ac", "all", 1, "", "" );

            ub xml = new ub();
            this.mainPage.Application.Remove( xmlPath );//清缓存
            xml.ReloadSub( xmlPath );                   //加载配置

            if( Utils.ToSChinese( ac ) == "确定修改" )
            {
                string SSEMin = Utils.GetRequest( "SSEMin", "post", 2, @"^[0-9]\d*$", "【每注最小金额】填写错误" );
                string SSEMax = Utils.GetRequest( "SSEMax", "post", 2, @"^[0-9]\d*$", "【每注最大金额】填写错误" );
                string SSEBig = Utils.GetRequest( "SSEBig", "post", 2, @"^[0-9]\d*$", "【每期最大投注金额】填写错误" );
                string SSEPrizeCalcRate = Utils.GetRequest( "SSEPrizeCalcRate", "post", 2, @"^(0([\.]\d*[0-9]+)|0|1)$", "【奖金计算比例】填写错误" );
                string SSEPrizePoundageRate = Utils.GetRequest( "SSEPrizePoundageRate", "post", 2, @"^(0([\.]\d*[0-9]+)|0|1)$", "【兑奖手续费】填写错误" );
                string SSEGetPrizeTimeLimit = Utils.GetRequest( "SSEGetPrizeTimeLimit", "post", 2, @"^\d*$", "【兑奖有效期】填写错误" );
                //string SSEBegin = Utils.GetRequest( "SSEBegin", "post", 2, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", "【投注限制开始时间】填写错误" );
                //string SSEEnd = Utils.GetRequest( "SSEEnd", "post", 2, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", "【投注限制结束时间】填写错误" );
                string SSEPageSize = Utils.GetRequest( "SSEPageSize", "post", 2, @"^\d*$", "【前台数据分页长度】填写错误" );
                string SSEManagePageSize = Utils.GetRequest( "SSEManagePageSize", "post", 2, @"^\d*$", "【后台数据分页长度】填写错误" );
                string DefaultFastVal = Utils.GetRequest( "DefaultFastVal", "post", 1, @"^\d*#?\d*#?\d*#?\d*#?\d*$", "【默认快捷投注】格式错误" );
                string SSEBuyExpir = Utils.GetRequest( "SSEBuyExpir", "post", 2, @"^\d*$", "【防刷屏时间】填写错误" );
                string SFrule = Utils.GetRequest( "SFrule", "post", 1, "", "" );
                string SSEBeginDefault = Utils.GetRequest( "SSEBeginDefault", "post", 1, "", "" );
                string SSEEndDefault = Utils.GetRequest( "SSEEndDefault", "post", 1, "", "" );
                string SSEOpenDefault = Utils.GetRequest( "SSEOpenDefault", "post", 1, "", "" );

                if( Regex.IsMatch( SSEBeginDefault, @"^\d{2}:\d{2}:\d{2}$" ) == false )
                    Utils.Error( "开盘时间格式填写错误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                if( Regex.IsMatch( SSEEndDefault, @"^\d{2}:\d{2}:\d{2}$" ) == false )
                    Utils.Error( "截止时间格式填写错误", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                if( Regex.IsMatch( SSEOpenDefault, @"^\d{2}:\d{2}:\d{2}$" ) == false )
                    Utils.Error( "开奖时间格式填写错误", Utils.getUrl( this.mSseVersionMgr.pageName ) );


                xml.dss[ "SSEMin" ] = SSEMin;
                xml.dss[ "SSEMax" ] = SSEMax;
                xml.dss[ "SSEBig" ] = SSEBig;
                xml.dss[ "SSEPrizeCalcRate" ] = SSEPrizeCalcRate;
                xml.dss[ "SSEPrizePoundageRate" ] = SSEPrizePoundageRate;
                xml.dss[ "SSEGetPrizeTimeLimit" ] = SSEGetPrizeTimeLimit;
                //xml.dss[ "SSEBegin" ] = SSEBegin;
                //xml.dss[ "SSEEnd" ] = SSEEnd;
                xml.dss[ "SSEPageSize" ] = SSEPageSize;
                xml.dss[ "SSEManagePageSize" ] = SSEManagePageSize;
                xml.dss[ "DefaultFastVal" ] = DefaultFastVal;
                xml.dss[ "SSEBuyExpir" ] = SSEBuyExpir;
                xml.dss[ "SFrule" ] = SFrule;
                xml.dss[ "SSEBeginDefault" ] = SSEBeginDefault;
                xml.dss[ "SSEEndDefault" ] = SSEEndDefault;
                xml.dss[ "SSEOpenDefault" ] = SSEOpenDefault;

                System.IO.File.WriteAllText( this.mainPage.Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 );
                Utils.Success( "上证指数设置", "设置成功，正在返回..", Utils.getUrl( this.mSseVersionMgr.pageName ), "1" );


                return;
            }


            //显示配置项
            string strText = "每注最小金额(最小1币起):/,每注最大金额(0不限制) :/,每期最大投注金额(0不限制):/,奖金计算比例(默认0.9)：:/,兑奖手续费(默认0.1)/,兑奖有效期:/,前台分页数据页长(行):/,后台分页数据页长(行):/,快捷投注默认值(#号分隔：最多5个)/,防刷屏时间:/,玩法设置(支持UBB):/,默认开盘时间(格式:09:00:00)/,默认截止时间(格式:09:00:00)/,默认开奖时间(格式:09:00:00)/,backurl";
            string strName = "SSEMin,SSEMax,SSEBig,SSEPrizeCalcRate,SSEPrizePoundageRate,SSEGetPrizeTimeLimit,SSEPageSize,SSEManagePageSize,DefaultFastVal,SSEBuyExpir,SFrule,SSEBeginDefault,SSEEndDefault,SSEOpenDefault,backurl";
            string strType = "num,num,num,text,text,num,num,num,text,num,textarea,text,text,text,hidden";
            string strValu = "" + xml.dss[ "SSEMin" ] + "'" + xml.dss[ "SSEMax" ] + "'" + xml.dss[ "SSEBig" ] + "'" + xml.dss[ "SSEPrizeCalcRate" ] + "'" + xml.dss[ "SSEPrizePoundageRate" ] + "'" + xml.dss[ "SSEGetPrizeTimeLimit" ] + "'" + xml.dss[ "SSEPageSize" ] + "'" + xml.dss[ "SSEManagePageSize" ] + "'" + xml.dss[ "DefaultFastVal" ] + "'" + xml.dss[ "SSEBuyExpir" ] + "'" + xml.dss[ "SFrule" ] + "'" + xml.dss[ "SSEBeginDefault" ] + "'" + xml.dss[ "SSEEndDefault" ] + "'" + xml.dss[ "SSEOpenDefault" ] + "'" + Utils.getPage( 0 ) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,"+this.mSseVersionMgr.pageName+"act=setting,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );
            this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
            this.mainPage.builder.Append( "温馨提示:<br />游戏开放时间填写格式为:09:00:00<br />" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

            base.ShowPage();
        }
    }

      //重置游戏
    public class SSEPageOrderQuery : SSEPageOrder
    {
        private int ptype;

        public SSEPageOrderQuery( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            base.ShowPage();
        }
    }


   
    //重置游戏
    public class SSEPageGameReset : SseManagePageBase
    {
        private int ptype;

        public SSEPageGameReset( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            //int ManageId = new BCW.User.Manage().IsManageLogin();
            //if( ManageId != 1 && ManageId != 9 )
            //{
            //    Utils.Error( "权限不足", "" );
            //}
                

            //IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster ) );
            this.baseMaster.Title = "重置游戏";
            this.SetTitle( "重置游戏" );       
                                         
            this.mainPage.builder.Append( Out.Tab( "<div class=\"test\">", "" ) );
            this.mainPage.builder.Append( "<b style=\"color:#ff0000\">重置游戏将会清空所有数据，请谨慎操作！</b><br />" );


            DateTime _openTime = DateTime.Parse( ub.GetSub( "SSEBegin", xmlPath ) );
            
            ptype = Utils.ParseInt( Utils.GetRequest( "clearTag", "post", 1, "", "" ) );
            if( ptype == 1 )
            {
                //执行删除数据.
                IDataParameter[] _parameters = new SqlParameter[0];
                BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseReset", _parameters );


                //获取新一期的开奖数据.
                BCW.SSE.SSEDataParseNew _c = new BCW.SSE.SSEDataParseNew();
                _c.autoOpenPrize = false;
                if( _c.GetNewSSEData() == true )
                    SetNextSseTime();
                       

                Utils.Success( "重置游戏", "重置成功", Utils.getUrl( this.mSseVersionMgr.pageName ), "1" );
               // this.mainPage.builder.Append( "重置成功<br />" );
                return;
            }

            string strText = ",";
            string strName = "clearTag,backurl";
            string strType = "hidden,hidden";
            string strValu = "" + "1'" + Utils.getPage( 0 ) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定重置/,"+this.mSseVersionMgr.pageName+"act=reset,post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );

            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName+"act=main" ) + "\">以后再说吧</a> <br />" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
            base.ShowPage();
        }


        private void SetNextSseTime()
        {
            DateTime _openTime = DateTime.Parse( DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() +" " + ub.GetSub( "SSEBeginDefault", xmlPath ) );


            DateTime _tempcloseTime = _openTime; 
            //跳过星期六日.                 
             if (_tempcloseTime.DayOfWeek == DayOfWeek.Friday)
                 _tempcloseTime = _tempcloseTime.AddDays(3);
             else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                  _tempcloseTime = _tempcloseTime.AddDays(2);
             else
                 _tempcloseTime = _tempcloseTime.AddDays(1);
             DateTime _closeTime = DateTime.Parse( _tempcloseTime.Year.ToString() + "-" + _tempcloseTime.Month.ToString() + "-" + _tempcloseTime.Day.ToString() + " " + ub.GetSub( "SSEEndDefault", xmlPath ) );

             DateTime _openPrizeTime = DateTime.Parse( _closeTime.Year.ToString() + "-" + _closeTime.Month.ToString() + "-" + _closeTime.Day.ToString() + " " + ub.GetSub( "SSEOpenDefault", xmlPath ) );

            ub xml = new ub();
            this.mainPage.Application.Remove( xmlPath );//清缓存
            xml.ReloadSub( xmlPath );                   //加载配置  

            xml.dss[ "SSEBegin" ] = _openTime.ToString( "yyyy-MM-dd HH:mm:ss" );
            xml.dss[ "SSEEnd" ] = _closeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
            xml.dss[ "SSEOpen" ] = _openPrizeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
            System.IO.File.WriteAllText( this.mainPage.Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 );  
        }
    }


    #region 确认兑奖
    public class SSEPageGetPirzeConfirm : SseManagePageBase
    {
        private int id = 0;
        protected string strText = string.Empty;
        protected string strName = string.Empty;
        protected string strType = string.Empty;
        protected string strValu = string.Empty;
        protected string strEmpt = string.Empty;
        protected string strIdea = string.Empty;
        protected string strOthe = string.Empty;

        public SSEPageGetPirzeConfirm( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }


        public override void ShowPage()
        {
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster,this.mSseVersionMgr ) );

            this.baseMaster.Title = "确认兑奖";


            this.id = int.Parse( Utils.GetRequest( "prizeId", "get", 1, @"^[0-9]\d*$", "0" ) );
            //this.id = Utils.ParseInt( );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );




            DataSet _ds = BCW.Data.SqlHelper.Query( string.Format( "select * from  ve_SseGetPrize where id={0}", this.id ) );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {
                if( _ds.Tables[ 0 ].Rows[ 0 ][ "isGet" ].ToString().Trim() == "True" )
                {
                    Utils.Error( "该会员已经兑过奖了", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                }

                this.mainPage.builder.Append( "请您确认以下兑奖信息 <br />" );

                string _sseNo = _ds.Tables[ 0 ].Rows[ 0 ][ "sseNo" ].ToString();
                bool _buyType = bool.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "buyType" ].ToString() );
                decimal _buyMoney =  decimal.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "buyMoney" ].ToString() );
                decimal _getPrize =  decimal.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "prizeVal" ].ToString() );
                decimal _poundage =  decimal.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "poundage" ].ToString() );
                decimal _getRealPrize = _getPrize - _poundage;
                DateTime _lastGetDateTime = DateTime.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "openDateTime" ].ToString() ).AddDays( int.Parse( ub.GetSub( "SSEGetPrizeTimeLimit", xmlPath ) ) );

                this.mainPage.builder.Append( string.Format( "第【{0}】期<br />", _sseNo ) );
                this.mainPage.builder.Append( string.Format( "投注类型：【{0}】<br />", _buyType == true ? "猜涨" : "猜跌" ) );
                this.mainPage.builder.Append( string.Format( "投注金额：{0}{1}<br />", _buyMoney.ToString( "#0" ), this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "中奖金额：{0}{1}<br />", _getPrize.ToString( "#0" ), this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "兑奖手费费：{0}{1}<br />", _poundage.ToString( "#0" ), this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "实发金额：{0}{1}<br />", _getRealPrize.ToString( "#0" ), this.mSseVersionMgr.account.name ) );
                this.mainPage.builder.Append( string.Format( "最后兑奖期限：{0}<br />", _lastGetDateTime ) );
                this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );


                this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );

                strText = ",,,,,,";
                strName = "sseNo,prizeId,getMoney,buyType,poundage,act,backurl";
                strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "" + _sseNo + "'" + this.id + "'" + _getRealPrize.ToString("#") + "'" + _buyType + "'" + _poundage + "'getPirzeSubMit'" + Utils.PostPage( 1 ) + "";
                strEmpt = "false,false,false,false,false,false";
                strIdea = "";
                strOthe = string.Format( " 确认兑奖,{0},post,4,other", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) + "<br />" );

            }
            else
            {
                Utils.Error( "兑奖信息无效或已过期", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }

            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster ,this.mSseVersionMgr) );
        }
    }
    #endregion


    #region 兑奖提交
    public class SSEPageGetPirzeSubmit : SseManagePageBase
    {

        public SSEPageGetPirzeSubmit( Manage_game_sse _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            this.baseMaster.Title = "确认兑奖";
            this.SetTitle( "确认兑奖" );    

            int _sseNo = int.Parse( Utils.GetRequest( "sseNo", "post", 4, @"^[0-9]\d*$", "无法获取期号" ) );
            int _prizeId = int.Parse( Utils.GetRequest( "prizeId", "post", 4, @"^[0-9]\d*$", "找不到兑奖信息" ) );
            long _getMoney = ( long ) decimal.Parse( Utils.GetRequest( "getMoney", "post", 1, "", "" ) );

            bool _buyType = bool.Parse( Utils.GetRequest( "buyType", "post", 1, "", "" ) );
            long _poundage = ( long ) decimal.Parse( Utils.GetRequest( "poundage", "post", 1, "", "" ) );

            //注册手工注册防刷
            int Expir = Utils.ParseInt( ub.GetSub( "SSEBuyExpir", xmlPath ) );
            string CacheKey = Utils.GetUsIP() + "_sse";
            object getObjCacheTime = DataCache.GetCache( CacheKey );
            if( getObjCacheTime != null )
            {
                Utils.Error( ub.GetSub( "BbsGreet", "/Controls/bbs.xml" ), Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }
            object ObjCacheTime = DateTime.Now;
            DataCache.SetCache( CacheKey, ObjCacheTime, DateTime.Now.AddSeconds( Expir ), TimeSpan.Zero );


            DataSet _ds = BCW.Data.SqlHelper.Query( string.Format( "select * from  ve_SseGetPrize where id={0}", _prizeId ) );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {
                if( _ds.Tables[ 0 ].Rows[ 0 ][ "isGet" ].ToString().Trim() == "True" )
                {
                    Utils.Error( "该会员已经兑过奖了", Utils.getUrl( "sse.aspx" ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                }
            }

            //执行兑奖.
            int _id = BCW.SSE.SSE.Instance().GetPrize( _prizeId, _getMoney );

            //获取会员Id
            BCW.SSE.Model.SseGetPrize _modGetPrize = new BCW.SSE.BLL.SseGetPrize().GetModel( _prizeId );

            if( _id > 0 & _modGetPrize != null )
            {

                //扣币
                this.mSseVersionMgr.account.UpdateiGold( _modGetPrize.userId, _getMoney, string.Format( "上证【{0}】期兑奖|标识({1})", _sseNo, _id ) );

               // new BCW.BLL.User().UpdateiGold( _modGetPrize.userId, _getMoney, string.Format( "上证【{0}】期兑奖|标识({1})", _sseNo, _id ) );

                //Utils.Success( "下注", "下注成功，花费了" + _money + "" + this.mSseVersionMgr.account.name + "<br />", Utils.getUrl( this.mSseVersionMgr.pageName+"act=myOrder" ), "2" );
                Utils.Success( "兑奖", "兑奖成功，获得了" + _getMoney.ToString( "#0" ) + "" + this.mSseVersionMgr.account.name + "<br />", Utils.getUrl( this.mSseVersionMgr.pageName+"act=orderQuery" ), "2" );

            }
            else
                Utils.Error( "兑奖失败", Utils.getUrl(this.mSseVersionMgr.pageName+"act=orderQuery") );


            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

            this.mainPage.builder.Append( string.Format( "{0}|{1}|{2}<br />", "确认兑奖", _prizeId.ToString(), _getMoney.ToString() ) );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
        }

    }
    #endregion
    
}