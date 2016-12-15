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
using System.Reflection;
using BCW.SSE;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using BCW.SSE.Class.sseMgr;


public partial class bbs_game_SSE : System.Web.UI.Page
{
    public System.Text.StringBuilder builder = new System.Text.StringBuilder("");     

    protected void Page_Load(object sender, EventArgs e)
    {


        //SSEPageMain _main = new SSEPageMain(this, this.Master);
        //_main.ShowPage();

        string _act = Utils.GetRequest("act", "all", 1, "", "");

        //选择运行版本(酷币版Or金币版)
        int _sseType = Utils.ParseInt(Utils.GetRequest( "sseVe", "all", 1, "", "-1" ));
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
        

        SSEPage.SSEPageBase _pageObj = null;
        _pageObj = null;
        switch (_act)
        {
            case "hPirze":
                _pageObj = new SSEPage.SSEPagePrizeHistory( this, this.Master, _verMgr );          // 分版完成
                break;
            case "order":
                _pageObj = new SSEPage.SSEPageOrder( this, this.Master, _verMgr );                  // 分版完成
                break;
            case "myOrder":
                _pageObj = new SSEPage.SSEPageMyOrder( this, this.Master, _verMgr );               // 分版完成
                break;
            case "sseOrderConfirm":
                _pageObj = new SSEPage.SSEPageOrderConfirm( this, this.Master, _verMgr );          // 分版完成
                break;
            case "sseOrderSubmit":
                _pageObj = new SSEPage.SSEPageOrderSubmit( this, this.Master, _verMgr );          // 分版完成
                break;
            case "rule":
                _pageObj = new SSEPage.SSEPageExplain( this, this.Master, _verMgr );              //无需分版
                break;
            case "getPirze":
                _pageObj = new SSEPage.SSEPageGetPirze( this, this.Master, _verMgr );            // 分版完成
                break;
            case "getPirzeConfirm":
                _pageObj = new SSEPage.SSEPageGetPirzeConfirm( this, this.Master, _verMgr );      // 分版完成
                break;
            case "getPirzeSubMit":
                _pageObj = new SSEPage.SSEPageGetPirzeSubmit( this, this.Master, _verMgr );       // 分版完成
                break;
            case "charts":
                _pageObj = new SSEPage.SSEPageCharts( this, this.Master, _verMgr );                 // 分版完成
                break;
            case "fastVal":
                _pageObj = new SSEPage.SSEPageFastVal( this, this.Master, _verMgr );                //无需分版
                break;
            case "prizepool":
                _pageObj = new SSEPage.SSEPagePrizepool( this, this.Master, _verMgr );              //分版完成
                break;
            default:
                _pageObj = new SSEPage.SSEPageOrder( this, this.Master, _verMgr );                 // 分版完成
                break;
        }
        _pageObj.ShowPage();
    }
}

namespace SSEPage
{
    public abstract class SSEPageBase
    {
        protected bbs_game_SSE mainPage;
        protected BCW.Common.BaseMaster baseMaster;
        protected string xmlPath = "/Controls/SSE.xml";
        protected int meid;
        protected int pageSize;

        protected sseMgrBase mSseVersionMgr;


        public SSEPageBase(bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster,sseMgrBase _sseMgr)
        {
            this.mainPage = _mainPage;
            this.baseMaster = _baseMaster;
            meid = new BCW.User.Users().GetUsId();
            pageSize = int.Parse(ub.GetSub("SSEPageSize", xmlPath));
            this.mSseVersionMgr = _sseMgr;
        }

        public abstract void ShowPage();

        public void IncludePage(SSEPageBase _page)
        {
            if (_page != null)
                _page.ShowPage();
        }

        public sseMgrBase verMgr
        {
            get
            {
                return mSseVersionMgr;
            }
        }
    }


    /// <summary>
    ///页头类
    /// </summary>
    ///
    public class SSEPageHeader : SSEPageBase
    {


        public SSEPageHeader(bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster,sseMgrBase _sseMgr)
            : base(_mainPage, _baseMaster, _sseMgr)
        {

        }

        public override void ShowPage()
        {
           

            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            //判断游戏状态.
            if (ub.GetSub("SSEState", xmlPath) == "0")
            {
                Utils.Safe("游戏");
                return;
            }

            //是否在内测阶段.
            if (ub.GetSub("SSEState", xmlPath) == "1")
            {
                string SSEDemoIDS = ub.GetSub("SSEDemoIDS", xmlPath);

                List<string> _lstId = new List<string>(Regex.Split(SSEDemoIDS, "#")); ;
                if (_lstId.IndexOf(new BCW.User.Users().GetUsId().ToString()) < 0)
                {
                    Utils.Error( "抱歉.你还没有获得内测资格。", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                }
            }


            this.mainPage.builder.Append(Out.Tab("<div class=\"title\">", ""));
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( "default.aspx" ) + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=order" ) + "\">上证指数【" + this.mSseVersionMgr.account.name + "版】</a>" );
            this.mainPage.builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>");
            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));

            this.baseMaster.Title = ub.GetSub("SSEName", xmlPath);
            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
            //this.mainPage.builder.Append("现价：{0} <br />");
             this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );  
            
            this.mainPage.builder.Append( string.Format( "{0} <br />", BCW.SSE.SSE.Instance().GetEndOrderTime() ) );


            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=getPirze" ) + "\">兑奖 </a>." );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=myOrder" ) + "\">记录 </a>." );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=rule" ) + "\">玩法 </a>." );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=charts" ) + "\">排行 </a>." );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=hPirze" ) + "\">开奖 </a>." );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=prizepool" ) + "\">奖池</a> " );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
            this.mainPage.builder.Append( string.Format( "我的"+this.mSseVersionMgr.account.name+"：{0}", Utils.ConvertGold( this.mSseVersionMgr.account.GetGold() ) + "" + this.mSseVersionMgr.account.name + "" ) + " <br />" );             
            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    //页脚类.
    public class SSEPageFooter : SSEPageBase
    {

        public SSEPageFooter( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.mainPage.builder.Append( Out.Tab( "<div class=\"title\">", Out.Hr() ) );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( "/default.aspx" ) + "\">首页</a>-" );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( "default.aspx" ) + "\">游戏大厅</a>" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
        }
    }

    //最新动态
    public class SSEPageNews : SSEPageBase
    {


        public SSEPageNews( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.mainPage.builder.Append(Out.Tab("<div class=\"text\">", ""));
            this.mainPage.builder.Append( "〓游戏动态〓" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );


            // 开始读取动态列表
            int SizeNum = 5;
            string strWhere = "";
            strWhere = "Types=1021";
            IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions( SizeNum, strWhere );
            if( listAction.Count > 0 )
            {
                int k = 1;
                foreach( BCW.Model.Action n in listAction )
                {
                    this.mainPage.builder.Append( Out.Tab( "<div>", "<br />" ) );
                    string ForNotes = Regex.Replace( n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase );
                    //      ForNotes = ForNotes.Replace("幸运28", "");
                    this.mainPage.builder.AppendFormat( "{0}前{1}", DT.DateDiff2( DateTime.Now, n.AddTime ), Out.SysUBB( ForNotes ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                    k++;
                }
                if( k > SizeNum )
                {
                    this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
                    this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl( "/bbs/action.aspx?ptype=1021&amp;backurl=" + Utils.PostPage( 1 ) + "" ) + "\">更多动态&gt;&gt;</a>" );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }
            }


            //DataSet ds = BCW.Data.SqlHelper.Query(string.Format("select top 5 * from tb_SseOrder where orderType =0 order by sseNo desc,orderDateTime desc"));

            //if (ds != null && ds.Tables[0].Rows.Count > 0)
            //{

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        int _userId = int.Parse(ds.Tables[0].Rows[i]["userId"].ToString());
            //        int _sseNo = int.Parse(ds.Tables[0].Rows[i]["sseNo"].ToString());
            //        DateTime _dateTime = DateTime.Parse(ds.Tables[0].Rows[i]["orderDateTime"].ToString());
            //        bool _buyType = Convert.ToBoolean(ds.Tables[0].Rows[i]["buyType"].ToString());

            //        string _userName = new BCW.BLL.User().GetUsName(_userId)+"("+_userId+")";

            //        this.mainPage.builder.Append(string.Format("<a href=\"/bbs/uinfo.aspx?uid=" + _userId + "\">{0}</a>在【{1}】投注第{2}期,投注金额:**{3}", _userName, DateStringFromNow(_dateTime), _sseNo, ub.Get("SiteBz")) + "<br />");
            //    }
            //}                    

            //this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));
        }




        //时间段
        public string DateStringFromNow( DateTime dt )
        {
            TimeSpan span = DateTime.Now - dt;
            if( span.TotalDays > 60 )
            {
                return dt.ToShortDateString();
            }
            else
                if( span.TotalDays > 30 )
                {
                    return "1个月前";
                }
                else
                    if( span.TotalDays > 14 )
                    {
                        return "2周前";
                    }
                    else
                        if( span.TotalDays > 7 )
                        {
                            return "1周前";
                        }
                        else
                            if( span.TotalDays > 1 )
                            {
                                return string.Format( "{0}天前", ( int ) Math.Floor( span.TotalDays ) );
                            }
                            else
                                if( span.TotalHours > 1 )
                                {
                                    return string.Format( "{0}小时前", ( int ) Math.Floor( span.TotalHours ) );
                                }
                                else
                                    if( span.TotalMinutes > 1 )
                                    {
                                        return string.Format( "{0}分钟前", ( int ) Math.Floor( span.TotalMinutes ) );
                                    }
                                    else
                                        if( span.TotalSeconds >= 1 )
                                        {
                                            return string.Format( "{0}秒前", ( int ) Math.Floor( span.TotalSeconds ) );
                                        }
                                        else
                                        {
                                            return "1秒前";
                                        }
        }
    }


    //历史开奖
    public class SSEPagePrizeHistory : SSEPageBase
    {


        public SSEPagePrizeHistory( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            this.IncludePage(new SSEPageHeader(this.mainPage, this.baseMaster,this.verMgr));


            string _sseNo = Utils.GetRequest( "sseNo", "get", 1, "", "" );

            if( string.IsNullOrEmpty(_sseNo) == false )
            {
                this.ShowPrizeDetail( _sseNo );
                return;
            }


            this.mainPage.builder.Append(Out.Tab("<div class=\"text\">", ""));
            this.mainPage.builder.Append("〓开奖记录〓<br />");
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );


            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe", "act", "ptype", "backurl" };
            string strWhere = "";
            pageIndex = Utils.ParseInt(this.mainPage.Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // 开始读取列表
            IList<BCW.SSE.Model.SseBase> lstSseBase = new BCW.SSE.BLL.SseBase().GetSseBasePages(pageIndex, pageSize, strWhere, out recordCount);
            if (lstSseBase.Count > 0)
            {
                int k = 1;
                foreach (BCW.SSE.Model.SseBase n in lstSseBase)
                {
                    if (k % 2 == 0)
                        this.mainPage.builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            this.mainPage.builder.Append(Out.Tab("<div>", ""));
                        else
                            this.mainPage.builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=hPirze&amp;sseNo={0}" ) + "\">第{1}期：{2}点【{3}】</a>", n.sseNo, n.sseNo, n.closePrice.ToString(), n.bz.Trim() == "1" ? "涨" : n.bz.Trim() == "0" ? "跌" : "平" ) );
                    k++;
                    this.mainPage.builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append(Out.Div("div", "没有相关记录..."));
            }


            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
        }


        private void ShowPrizeDetail( string _sseNo )
        {
            //显示该期下中奖的信息.
           


            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe","act", "sseNo", "backurl" };
            string strWhere = "p.sseNo=" + _sseNo + " and p.orderType="+this.mSseVersionMgr.versionId;
            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;
            

            // 开始读取列表
            IList<BCW.SSE.Model.SseGetPrizeCharts> lstData = new BCW.SSE.BLL.SseGetPrize().GetSseGetPrizeChartsPages( pageIndex, pageSize, strWhere, out recordCount );
            if( lstData.Count > 0 )
            {
                int k = 1;


                DataSet _ds = new BCW.SSE.BLL.SseBase().GetList( " sseNo =" + _sseNo );
                if( _ds.Tables[ 0 ].Rows.Count > 0 )
                {
                    this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );  
                    this.mainPage.builder.Append( string.Format( "〓第{0}期〓<b style=\"color:#ff0000\">开奖：{1} 【{2}】</b><br />", _sseNo, _ds.Tables[ 0 ].Rows[ 0 ][ "closePrice" ], _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "1" ? "涨" : _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "-1" ? "平" : "跌" ) );
                
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                    this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );  
                    this.mainPage.builder.Append( string.Format( "共有{0}人次中奖<br />", lstData.Count ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                }
                    
                foreach( BCW.SSE.Model.SseGetPrizeCharts n in lstData )
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
                    this.mainPage.builder.Append( ( ( pageIndex - 1 ) * pageSize + k ) + "、<a href=\"" + Utils.getUrl( "/bbs/uinfo.aspx?uid=" + n.userId + "&amp;backurl=" + Utils.PostPage( 1 ) + "" ) + "\">" + BCW.User.Users.SetUser( n.userId ) + "(" + n.userId + ")" + "</a>" + "赢得" + n.prizeVal + "" + this.mSseVersionMgr.account.name + "" );
                    
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

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
        }         
    }

    //我要下注
    public class SSEPageOrder : SSEPageBase
    {
        protected string strText = string.Empty;
        protected string strName = string.Empty;
        protected string strType = string.Empty;
        protected string strValu = string.Empty;
        protected string strEmpt = string.Empty;
        protected string strIdea = string.Empty;
        protected string strOthe = string.Empty;

        private int sseNo;

        public SSEPageOrder( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.mainPage.Master.Title = "我要下注";
            sseNo = BCW.SSE.SSE.Instance().GetBuySseNo();

            this.IncludePage(new SSEPageHeader(this.mainPage, this.baseMaster,this.verMgr));

            this.mainPage.builder.Append( Out.Tab( "<div  class=\"text\">", "" ) );

            this.mainPage.builder.Append( "〓我要猜猜〓" );

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );

            List<string> _lstResult = BCW.SSE.SSE.Instance().GetFooterData(this.mSseVersionMgr.versionId);


            this.mainPage.builder.Append( string.Format( "第{0}期押注总金额：{1}{2}<br />", BCW.SSE.SSE.Instance().GetBuySseNo(), _lstResult[ 3 ].ToString().Split( '.' )[ 0 ], this.mSseVersionMgr.account.name + "" ) );

            //快速下注 
            this.mainPage.builder.Append( "快速下注：<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=fastVal" ) + "\">[设置]</a><br />" );
           
            GetFastVal();


            this.mainPage.builder.Append( string.Format( "本期猜涨总金额：{0}{1}:", _lstResult[ 1 ].ToString().Split( '.' )[ 0 ], this.mSseVersionMgr.account.name + "" ) );
            strText = ",,,,";
            strName = "buyType,money,act,sseNo,backurl";
            strType = "hidden,num,hidden,hidden,hidden";
            strValu = "1'" + "'sseOrderConfirm'" + sseNo.ToString() + "'" + Utils.PostPage( 1 ) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = string.Format( " 押涨,{0},post,4,other", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) + "<br />" );


            this.mainPage.builder.Append( string.Format( "本期猜跌总金额：{0}{1}:", _lstResult[ 2 ].ToString().Split( '.' )[ 0 ], this.mSseVersionMgr.account.name + "" ) );    
            strText = ",,,,";
            strName = "buyType,money,act,sseNo,backurl";
            strType = "hidden,num,hidden,hidden,hidden";
            strValu = "0'" + "'sseOrderConfirm'" + sseNo.ToString() + "'" + Utils.PostPage( 1 ) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = string.Format( " 押跌,{0},post,4,other", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) + "<br />" );

            this.mainPage.builder.Append( string.Format( "提示：限{0}-{1}", ub.GetSub( "SSEMin", this.xmlPath ) , ub.GetSub( "SSEMax", this.xmlPath ) + this.mSseVersionMgr.account.name ) );



            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            this.IncludePage( new SSEPageNews( this.mainPage, this.baseMaster, this.verMgr ) );
            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
        }

        //获取快捷下注列表
        private void GetFastVal()
        {
            //如果玩家没有自定则取后台默认设定
            BCW.SSE.Model.SseFastVal _modelFastVal = new BCW.SSE.BLL.SseFastVal().GetModel( meid );
            if( _modelFastVal == null )
            {
                _modelFastVal = new BCW.SSE.Model.SseFastVal();
                _modelFastVal.fastVal = ub.GetSub( "DefaultFastVal", xmlPath );                   
            }

            //生成超链接字符串
            string[] _arryFastVal = _modelFastVal.fastVal.Split( '#' );

           // _str ="<form id=\"forms\" method=\"post\" action=\ this.mSseVersionMgr.pageName + "sseOrderConfirm\">";

            
            for( int i = 1; i >=0; i-- )
            {
                this.mainPage.builder.Append( i == 0 ? "买跌：" : "买涨：" );
                int k = 0;
                foreach( string _val in _arryFastVal )
                {
                    strText = ",,,,";
                    strName = "buyType,money,act,sseNo,backurl";
                    strType = "hidden,hidden,hidden,hidden,hidden";
                    strValu = ""+i+"'"+ _val + "'sseOrderConfirm'" + sseNo.ToString() + "'" + Utils.PostPage( 1 ) + "";
                    strEmpt = "false,false,false,false,false";
                    strIdea = "";
                    string _a = k < _arryFastVal.Length - 1 ? "｜" : "";
                    strOthe = string.Format( " "+_val+_a+",{0},post,4,other", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                    this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) );

                    k++;

                }
                this.mainPage.builder.Append( "<br />" );
            }

            //_str +="</form\">";

        }
    }

    //确认下注.
    public class SSEPageOrderConfirm : SSEPageBase
    {
        protected string strText = string.Empty;
        protected string strName = string.Empty;
        protected string strType = string.Empty;
        protected string strValu = string.Empty;
        protected string strEmpt = string.Empty;
        protected string strIdea = string.Empty;
        protected string strOthe = string.Empty;

        public SSEPageOrderConfirm( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            //是否在下注时段内.
            //星期六、日全天不能下注(因为不开盘).

            if( BCW.SSE.SSE.Instance().CheckBuyValidity == false )
            {

                Utils.Error( "已截止下注", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }   


            this.mainPage.Master.Title = "确认下注";
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );


            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );
            this.mainPage.builder.Append( "〓确认下注〓" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            long _money = Int64.Parse(Utils.GetRequest("money", "post", 4, @"^[0-9]\d*$", "下注金额填写出错"));
            string _sseNo = Utils.GetRequest("sseNo", "post", 1, "", "");
            int _buyType = int.Parse(Utils.GetRequest("buyType", "post", 1, "", ""));

            //购买合法性检查.
            //是否超出本期最高额度.
            if( int.Parse( ub.GetSub( "SSEBig", this.xmlPath ) ) > 0 )
            {
                if( _money > BCW.SSE.SSE.Instance().residualAmount )
                {
                    Utils.Error( "下注金额不能超过剩余投注额度", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                }
            }

            if( int.Parse( ub.GetSub( "SSEMax", this.xmlPath ) ) > 0 )
            {
                if( _money > int.Parse( ub.GetSub( "SSEMax", this.xmlPath ) ) )
                {
                    Utils.Error( "下注金额不能大于本次最高投注额度", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                }
            }

            if( int.Parse( ub.GetSub( "SSEMin", this.xmlPath ) ) > 0 )
            {
                if( _money < int.Parse( ub.GetSub( "SSEMin", this.xmlPath ) ) )
                {
                    Utils.Error( "下注金额不能小于本次最低投注额度", Utils.getUrl(this.mSseVersionMgr.pageName) );
                }
            }

            if (this.mSseVersionMgr.account.GetGold() < _money)
            {
                Utils.Error( "对不起，你的" + this.mSseVersionMgr.account.name + "不足", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );

            this.mainPage.builder.Append(string.Format("下注期数：{0}", _sseNo) + " <br />");
            this.mainPage.builder.Append(string.Format("下注金额：{0}", _money+this.mSseVersionMgr.account.name) + " <br />");
            this.mainPage.builder.Append(string.Format("下注类型：买{0}", _buyType == 1 ? "涨" : "跌") + " <br />");               

     
            strText = ",,,,";
            string strName = "money,buyType,act,sseNo,backurl";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            strValu = _money + "'" + _buyType + "'sseOrderSubmit'" + _sseNo + "'" + this.mSseVersionMgr.account.name + "";
            strEmpt = "false,false,,false,false,false";
            // strIdea = "";
            strOthe = string.Format(" 确认下注,{0},post,4,other", Utils.getUrl(this.mSseVersionMgr.pageName));
            this.mainPage.builder.Append((Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe)).Replace("<form ", "<form style=\"display: inline\"; ") + "<br />");
            this.mainPage.builder.Append("<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=order") + "\">再看看吧</a> <br />");

            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );

        }

    }

    public class SSEPageOrderSubmit : SSEPageBase
    {


        public SSEPageOrderSubmit( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.mainPage.Master.Title = "确认下注";
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );    
           

            Int64 _money = Int64.Parse(Utils.GetRequest("money", "post", 4, @"^[0-9]\d*$", "下注金额填写出错"));
            int _sseNo = int.Parse(Utils.GetRequest("sseNo", "post", 1, "", ""));
            int _buyType = int.Parse(Utils.GetRequest("buyType", "post", 1, "", ""));


            //提交前再判断一次是否在可下注时间(防止用户在页面停留到边界)
            if(BCW.SSE.SSE.Instance().CheckBuyValidity == false || _sseNo != BCW.SSE.SSE.Instance().GetBuySseNo())
            {
                Utils.Error( "当前投注信息已过期", Utils.getUrl( Utils.getUrl( this.mSseVersionMgr.pageName  + "act=order") ) );
                return;
            }


            #region 付款
            //支付安全提示         
            string[] p_pageArr = { "sseNo", "buyType", "money", "act" };
            BCW.User.PaySafe.PaySafePage( meid, this.mSseVersionMgr.pageName, p_pageArr, "post", false );


            //

            #endregion

            //判断是否刷屏.
            string appName = "LIGHT_SSE";
            long small = Convert.ToInt64(ub.GetSub("SSEMin", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("SSEMax", xmlPath));

            int Expir = Utils.ParseInt(ub.GetSub("SSEBuyExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, _money, small, big);




            int _id = BCW.SSE.SSE.Instance().CreateNewOrder(this.mSseVersionMgr.versionId,_sseNo, _buyType, _money,false);

            if (_id > 0)
            {
                //变更奖池.                  
                IDataParameter[] _parameters = new SqlParameter[3];
                _parameters[0] = new SqlParameter("@orderId", SqlDbType.Int);
                _parameters[1] = new SqlParameter("@operType", SqlDbType.Int);
                _parameters[2] = new SqlParameter("@operUserId", SqlDbType.Int);  
                _parameters[0].Value = _id;
                _parameters[1].Value = 0;             //下注.   
                _parameters[2].Value = meid;     
                BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseChangePrizePool", _parameters );         

                //写入游戏动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid)+"("+meid+")" + "[/url]在[url=/bbs/game/"+this.mSseVersionMgr.pageName+"]上证指数第" + _sseNo + "期[/url]下注**" + this.mSseVersionMgr.account.name + "";//" + Convert.ToInt64(allBuyCent) + "
                new BCW.BLL.Action().Add(1021, _id, 0, "", wText);

                //扣币

                this.mSseVersionMgr.account.UpdateiGold( -_money, string.Format( "上证|{0}期|标识:{1}|【{2}】", _sseNo, _id, _buyType == 1 ? "猜涨" : "猜跌" ) );

                #region 活跃抽奖入口
                //活跃抽奖入口_20161212 啊光
                try
                {
                    string gamename = "新上证指数";//游戏名字 可修改成变量
                    //表中存在gamename记录
                    if( new BCW.BLL.tb_WinnersGame().ExistsGameName( gamename ) )
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if( _money > new BCW.BLL.tb_WinnersGame().GetPrice( gamename ) )
                        {
                            string TextForUbb = ( ub.GetSub( "TextForUbb", "/Controls/winners.xml" ) );//设置内线提示的文字
                            string WinnersGuessOpen = ( ub.GetSub( "WinnersGuessOpen", "/Controls/winners.xml" ) );//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll( 1, _id, meid, new BCW.BLL.User().GetUsName( meid ), gamename, 3 );
                            if( hit == 1 )//返回1中奖
                            {
                                //内线开关 1开
                                if( WinnersGuessOpen == "1" )
                                {
                                    //发内线到该ID
                                    new BCW.BLL.Guest().Add( 0, meid, new BCW.BLL.User().GetUsName( meid ), TextForUbb );
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                #endregion

                //new BCW.BLL.User().UpdateiGold( meid, -_money, string.Format( "上证|{0}期|标识:{1}|【{2}】", _sseNo, _id, _buyType == 1 ? "猜涨" : "猜跌" ) );

                Utils.Success("下注", "下注成功，花费了" + _money + "" + this.mSseVersionMgr.account.name + "<br />", Utils.getUrl( this.mSseVersionMgr.pageName + "act=myOrder"), "2");

            }
            else
                Utils.Error( "下注失败", Utils.getUrl( this.mSseVersionMgr.pageName ) );


            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
        }

        
    }



    //我的押注
    public class SSEPageMyOrder : SSEPageBase
    {
        private int ptype = 0;

        public SSEPageMyOrder( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

            this.mainPage.builder.Append("<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=myOrder&amp;ptype=1") + "\">未开投注</a> ");
            this.mainPage.builder.Append("<a href=\"" + Utils.getUrl( this.mSseVersionMgr.pageName + "act=myOrder&amp;ptype=2") + "\">历史投注</a> ");

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "0"));

            this.mainPage.Master.Title = string.Format("我的{0}投注", ptype == 0 ? "所有" : ptype == 1 ? "本期" : "往期");

            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe", "act", "ptype", "backurl" };
            string strWhere = string.Format( "o.userId ={0} and o.orderType={1}", meid ,this.mSseVersionMgr.versionId);
            if (ptype == 1)     //本期.
                strWhere = strWhere + "and o.sseNo=" + BCW.SSE.SSE.Instance().GetBuySseNo() + "and o.orderType="+this.mSseVersionMgr.versionId;
            else if (ptype == 2)
                strWhere = strWhere + "and o.sseNo!=" + BCW.SSE.SSE.Instance().GetBuySseNo() + "and o.orderType=" + this.mSseVersionMgr.versionId;


            pageIndex = Utils.ParseInt(this.mainPage.Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;


            // 开始读取列表
            IList<BCW.SSE.Model.sseOrderHistory> lstSseOrder = new BCW.SSE.BLL.SseOrder().GetSseOrderHistoryPages(pageIndex, pageSize, strWhere, out recordCount);
            if (lstSseOrder.Count > 0)
            {
                int k = 1;
                int _lastSseNo = 0 ;
                int _count = 1;
                foreach( BCW.SSE.Model.sseOrderHistory n in lstSseOrder )
                {
                     

                    if (k % 2 == 0)
                        this.mainPage.builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            this.mainPage.builder.Append(Out.Tab("<div>", ""));
                        else
                            this.mainPage.builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    if( _lastSseNo != n.sseOrder.sseNo )
                    {
                        _lastSseNo = n.sseOrder.sseNo;
                        _count = 1;

                        DataSet _ds = new BCW.SSE.BLL.SseBase().GetList( " sseNo =" + _lastSseNo );
                        if( _ds.Tables[ 0 ].Rows.Count > 0 )
                            this.mainPage.builder.Append( string.Format( "〓第{0}期〓<b style=\"color:#ff0000\">开奖：{1} 【{2}】</b>", _lastSseNo, _ds.Tables[ 0 ].Rows[ 0 ][ "closePrice" ], _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "1" ? "涨" : _ds.Tables[ 0 ].Rows[ 0 ][ "bz" ].ToString().Trim() == "-1" ? "平" : "跌" ) );
                         else
                            this.mainPage.builder.Append( string.Format( "〓第{0}期〓<b style=\"color:#ff0000\">未开奖</b>", _lastSseNo ) );

                        this.mainPage.builder.Append( "<br />" );
                    }

                    this.mainPage.builder.Append( string.Format( "{0}.{1},金额:{2}，标识{3},{4}【{5}】", _count, n.sseOrder.buyType == false ? "猜跌" : "猜涨", n.sseOrder.buyMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.sseOrder.id, n.backMoney > 0 ? "<b style=\"color:#ff0000\">赢" + n.backMoney .ToString("#0")+"</b>": "", n.sseOrder.orderDateTime ) );

                    //this.mainPage.builder.Append( string.Format( "你于【{0}】投注第{1}期,投注类型【猜{2}】,投注金额:{3}{4}", n.orderDateTime, n.sseNo.ToString(), n.buyType == false ? "跌" : "涨", n.buyMoney, this.mSseVersionMgr.account.name )  );
                    if( n.sseOrder.state == 1)
                        this.mainPage.builder.Append( "<b style=\"color:red\">【已退注】</b>" );
                    k++;
                    _count++;
                    this.mainPage.builder.Append(Out.Tab("</div>", ""));


                }

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append(Out.Div("div", "没有相关记录..."));
            }


            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );

        }
    }


    
    #region 玩法说明
    public class SSEPageExplain : SSEPageBase
    {
        private int ptype = 0;

        public SSEPageExplain( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );
            this.mainPage.builder.Append( "〓玩法规则〓" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            this.mainPage.builder.Append(Out.Tab("<div>", ""));
            string _explainStr = ub.GetSub("SFrule", xmlPath).Replace("\\", "<br/>");//.Replace(" ", "&nbsp;");   

            this.mainPage.builder.Append(Out.SysUBB(_explainStr));

            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );

        }
    }
    #endregion


    #region 我要兑奖
    public class SSEPageGetPirze : SSEPageBase
    {
        private int ptype = 0;

        public SSEPageGetPirze( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

           // this.mainPage.builder.Append(Out.Tab("<div class=\"title\">", ""));

            this.baseMaster.Title = "我要兑奖";

            this.mainPage.builder.Append(Out.Tab("<div class=\"text\">", ""));

            this.mainPage.builder.Append( string.Format( "温馨提示：逾期{0}天未兑奖，则视为放弃<br />", ub.GetSub( "SSEGetPrizeTimeLimit", xmlPath ) ) );

            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=getPirze&amp;ptype=0" ) + "\">未兑奖</a> " );
            this.mainPage.builder.Append( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=getPirze&amp;ptype=1" ) + "\">已兑奖</a> " );

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            ptype = Utils.ParseInt( Utils.GetRequest( "ptype", "get", 1, @"^[1-2]$", "0" ) );



            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "sseVe", "act", "ptype", "backurl" };
            string strWhere = string.Format( "userId ={0} and isGet={1} and orderType={2}", meid, ptype.ToString(),this.mSseVersionMgr.versionId );
            if (ptype == 0)  //只显示N天内的可兑奖.
                  strWhere = strWhere + string.Format(" and openDateTime>='{0}'",System.DateTime.Now.AddDays(-int.Parse(ub.GetSub("SSEGetPrizeTimeLimit",xmlPath))));


            pageIndex = Utils.ParseInt( this.mainPage.Request.QueryString[ "page" ] );
            if( pageIndex == 0 )
                pageIndex = 1;

            // 开始读取列表
            int _index = 1;
            IList<BCW.SSE.Model.veSseGetPrize> lstSseGetPrize = new BCW.SSE.BLL.SseGetPrize().GetSseGetPrizePages( pageIndex, pageSize, strWhere, out recordCount );
            if( lstSseGetPrize.Count > 0 )
            {     


                int k = 1;
                foreach( BCW.SSE.Model.veSseGetPrize n in lstSseGetPrize )
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

                  // 猜涨.1,金额:1000.0000酷币，标识1141；【2016/11/22 16:10:42】
                   this.mainPage.builder.Append( string.Format( "{0}、 第{1}期/{2}/投注金额：{3}/中奖金额{4}",
                       _index,
                       n.sseNo,
                       n.buyType == false ? "猜跌" : "猜涨",
                       n.buyMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name,
                       n.prizeVal.ToString( "#0" )+ this.mSseVersionMgr.account.name+"  "));
                   if( ptype == 0 )
                   {
                       string strText = ",";
                       string strName = "prizeId,backurl";
                       string strType = "hidden,hidden";
                       string strValu = n.id + "'" + Utils.PostPage( 1 ) + "";
                       string strEmpt = "false,false";
                       string strIdea = "";
                       string strOthe = "我要兑奖,"+Utils.getUrl(this.mSseVersionMgr.pageName+"act=getPirzeConfirm")+",post,4,red";
                       this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " )  );
                   }
                      // ptype == 0 ? "【<a href=\"" + Utils.getUrl( "SSE.aspx?act=getPirzeConfirm&amp;prizeId=" + n.id ) + "\">我要兑奖</a>】" : "" ) );
                    //this.mainPage.builder.Append( string.Format( "您花费{0}购买的第【{1}】期【{2}】,共中得奖金{3}{4}", n.buyMoney.ToString() + ub.Get( "SiteBz" ), n.sseNo.ToString(), n.buyType == false ? "猜跌" : "猜涨", n.prizeVal.ToString( "#0.00" ), ptype == 0 ? "【<a href=\"" + Utils.getUrl( "SSE.aspx?act=getPirzeConfirm&amp;prizeId=" + n.id ) + "\">我要兑奖</a>】" : "" ) );
                    k++;
                    _index++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }

                // 分页
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }
                

            this.mainPage.builder.Append(Out.Tab("</div>", "<br />"));

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );

        }
    }
    #endregion
    


    #region 确认兑奖
    public class SSEPageGetPirzeConfirm : SSEPageBase
    {
        private int id = 0;
        protected string strText = string.Empty;
        protected string strName = string.Empty;
        protected string strType = string.Empty;
        protected string strValu = string.Empty;
        protected string strEmpt = string.Empty;
        protected string strIdea = string.Empty;
        protected string strOthe = string.Empty;

        public SSEPageGetPirzeConfirm( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }


        public override void ShowPage()
        {
            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

            this.baseMaster.Title = "确认兑奖";
                                 
            
            this.id = int.Parse(Utils.GetRequest( "prizeId", "post", 1, @"^[0-9]\d*$", "0" ));
            //this.id = Utils.ParseInt( );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );                

 


            DataSet _ds = BCW.Data.SqlHelper.Query( string.Format( "select * from  ve_SseGetPrize where id={0}", this.id) );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {
                if( _ds.Tables[ 0 ].Rows[ 0 ][ "isGet" ].ToString().Trim() == "True" )
                {
                    Utils.Error( "你已经兑过奖了", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                }

                this.mainPage.builder.Append( "请您确认以下兑奖信息:" + _ds.Tables[ 0 ].Rows[ 0 ][ "buyType" ].ToString() + " <br />" );
 
                string _sseNo = _ds.Tables[ 0 ].Rows[ 0 ][ "sseNo" ].ToString();
                bool _buyType = bool.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "buyType" ].ToString() );
                decimal _buyMoney = decimal.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "buyMoney" ].ToString() );
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
                strValu = "" + _sseNo + "'" + this.id + "'" + _getRealPrize.ToString("#0") + "'" + _buyType + "'" + _poundage + "'getPirzeSubMit'" + Utils.PostPage( 1 ) + "";
                strEmpt = "false,false,false,false,false,false";
                strIdea = "";
                strOthe = string.Format( " 确认兑奖,{0},post,4,red", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) + "<br />" );

            }
            else
            {
                Utils.Error( "兑奖信息无效或已过期", Utils.getUrl( this.mSseVersionMgr.pageName ) );
            }

            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
        }
    }
       #endregion


    #region 兑奖提交
    public class SSEPageGetPirzeSubmit : SSEPageBase
    {

        public SSEPageGetPirzeSubmit( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            this.baseMaster.Title = "确认兑奖";

            int _sseNo = int.Parse( Utils.GetRequest( "sseNo", "post", 4, @"^[0-9]\d*$", "无法获取期号" ));
            int _prizeId = int.Parse( Utils.GetRequest( "prizeId", "post", 4, @"^[0-9]\d*$", "找不到兑奖信息" ));
            long _getMoney = ( long )decimal.Parse(  Utils.GetRequest( "getMoney", "post", 1, "", "" ));

            bool _buyType = bool.Parse( Utils.GetRequest( "buyType", "post", 1, "", "" ) );
            long _poundage =( long )decimal.Parse( Utils.GetRequest( "poundage", "post", 1, "", "" ) );

            //注册手工注册防刷
            int Expir = Utils.ParseInt( ub.GetSub( "SSEBuyExpir", xmlPath ) );
            string CacheKey = Utils.GetUsIP()+"_sse";
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
                    Utils.Error( "你已经兑过奖了", Utils.getUrl( "SSE.aspx" ) );
                    this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
                }
            }

            //执行兑奖.
            int _id = BCW.SSE.SSE.Instance().GetPrize( _prizeId, _getMoney );

            if( _id > 0 )
            {

                //扣币
                this.mSseVersionMgr.account.UpdateiGold( _getMoney, string.Format( "上证【{0}】期兑奖|标识({1})", _sseNo, _id ) );

                //Utils.Success( "下注", "下注成功，花费了" + _money + "" + this.mSseVersionMgr.account.name + "<br />", Utils.getUrl(  this.mSseVersionMgr.pageName + "act=myOrder" ), "2" );
                Utils.Success( "兑奖","兑奖成功，获得了" + _getMoney.ToString("#0") + "" + this.mSseVersionMgr.account.name + "<br />", Utils.getUrl(  this.mSseVersionMgr.pageName + "act=getPirze" ), "2" );

            }
            else
                Utils.Error( "兑奖失败", Utils.getUrl( this.mSseVersionMgr.pageName  + "act=getPirze") );


            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );

            this.mainPage.builder.Append( string.Format( "{0}|{1}|{2}<br />", "确认兑奖",_prizeId.ToString(),_getMoney.ToString() ) );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );
        }

    }
        #endregion

        #region 兑奖提交
    public class SSEPageCharts : SSEPageBase
    {
        private int ptype;

        public SSEPageCharts( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {


            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

            this.baseMaster.Title = "排行榜";
                             
            ptype = Utils.ParseInt( Utils.GetRequest( "ptype", "get", 1, @"^[0-3]$", "0" ) );
            if( ptype <0 )
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

            //日期校验.
            if( ptype == 4 )
            {

            }

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
                    this.mainPage.builder.Append( "[第" + ( ( pageIndex - 1 ) * pageSize + k ) + "名]<a href=\"" + Utils.getUrl( "/bbs/uinfo.aspx?uid=" + n.userId + "&amp;backurl=" + Utils.PostPage( 1 ) + "" ) + "\">" + BCW.User.Users.SetUser( n.userId ) + "("+n.userId+")"+"</a>"  + "赢得" + n.prizeVal + "" + this.mSseVersionMgr.account.name + "" );
                    k++;
                    this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
                }
                if( recordCount >= 100 )
                {
                    this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0 ) );
                }
                else
                {
                    this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );
                }
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
            string strOthe = "马上查询/," + this.mSseVersionMgr.pageName + "act=charts" + ",post,1,red";
            this.mainPage.builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
           
  
        }
        
    }
        #endregion

    
     #region 快捷下注设置
    public class SSEPageFastVal : SSEPageBase
    {
        private int ptype;
        protected string strText = string.Empty;
        protected string strName = string.Empty;
        protected string strType = string.Empty;
        protected string strValu = string.Empty;
        protected string strEmpt = string.Empty;
        protected string strIdea = string.Empty;
        protected string strOthe = string.Empty;


        public SSEPageFastVal( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {

            this.IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr ) );

            this.baseMaster.Title = "快捷下注设置";


            string _faltVal = "";
            string _fastValStr = "";
            for( int k = 0; k <5; k++ )
            {
                _faltVal = Utils.GetRequest( "fastVal" + k.ToString(), "post", 1, "", "" );


                if( _faltVal != "" )
                {
                    //限制参数不能小于最低投注额或高于最高投注额
                    if( int.Parse( _faltVal ) < int.Parse( ub.GetSub( "SSEMin", xmlPath ) ) || int.Parse( _faltVal ) > int.Parse( ub.GetSub( "SSEMax", xmlPath ) ) )
                    {
                        Utils.Error( "参数" + ( k + 1 ).ToString() + "不能小于最低投注额或大于最高投注额", Utils.getUrl( this.mSseVersionMgr.pageName ) );
                        return;
                    }
                    _fastValStr += _faltVal;
                    if( k < 4 )
                        _fastValStr += "#";

                }
            }



            if( _faltVal != "" )
            {

                this.mainPage.builder.Append( _fastValStr );
                string[] _arryFastValSplit = _fastValStr.Split( '#' );

                //如果玩家没有自定则写入一个新的设定
                BCW.SSE.Model.SseFastVal _objFastVal = new BCW.SSE.BLL.SseFastVal().GetModel( meid );
                if( _objFastVal == null )
                {
                    _objFastVal = new BCW.SSE.Model.SseFastVal();
                    _objFastVal.userId = meid;
                    _objFastVal.fastVal = _fastValStr;
                    new BCW.SSE.BLL.SseFastVal().Add( _objFastVal );
                    Utils.Success( "保存", "保存成功", "", "1" );
                    return;
                }

                //存在则更新
                _objFastVal.userId = meid;
                _objFastVal.fastVal = _fastValStr;
                new BCW.SSE.BLL.SseFastVal().Update( _objFastVal );   
                
                Utils.Success( "保存", "保存成功", "", "1" );
            }


            //如果玩家没有自定则取后台默认设定
            BCW.SSE.Model.SseFastVal _modelFastVal = new BCW.SSE.BLL.SseFastVal().GetModel( meid );
            if( _modelFastVal == null )
            {
                _modelFastVal = new BCW.SSE.Model.SseFastVal();
                _modelFastVal.fastVal = ub.GetSub( "DefaultFastVal", xmlPath );
            }

            //生成超链接字符串
            string[] _arryFastVal = _modelFastVal.fastVal.Split( '#' );  


            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "<br />" ) );
            this.mainPage.builder.Append( "自定义快速下注：<br />" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );
            this.mainPage.builder.Append( string.Format( "温馨提示：(设置金额不能小于【{0}】且不能大于【{1}】)<br />", ub.GetSub( "SSEMin", xmlPath ) + this.mSseVersionMgr.account.name, ub.GetSub( "SSEMax", xmlPath ) + this.mSseVersionMgr.account.name ) );


            int i=0;
            foreach( string _val in _arryFastVal )
            {
                strText += string.Format("参数{0}:,/",i+1);
                strName += "fastVal"+i+",";
                strType += "num,";
                strValu += _val + "'";
                strEmpt += "true,";
                i++;  
            }

            strText += ",";
            strName += "act";
            strType += "hidden";
            strValu += "fastVal";
            strEmpt += "false";
            strIdea = "";
            strOthe = string.Format( "//保存设置/,{0},post,4,other", "" );
            this.mainPage.builder.Append( ( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) ).Replace( "<form ", "<form style=\"display: inline\"; " ) );

            this.mainPage.builder.Append( Out.Tab( "</div>", "" ) );

            this.IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );

        }
    }


    //奖池查询
    public class SSEPagePrizepool : SSEPageBase
    {

        private int ptype;              //页面类型
        private int pageIndex;
        private int recordCount;
        private string[] pageValUrl = { "sseVe","act", "ptype", "pSseNo", "backurl" };
        private string strWhere = "1=1";


        public SSEPagePrizepool( bbs_game_SSE _mainPage, BCW.Common.BaseMaster _baseMaster, sseMgrBase _sseMgr )
            : base( _mainPage, _baseMaster, _sseMgr )
        {

        }

        public override void ShowPage()
        {
            strWhere = "1=1 and p.prizeType=" + this.mSseVersionMgr.versionId;

            IncludePage( new SSEPageHeader( this.mainPage, this.baseMaster, this.verMgr) );

            this.mainPage.builder.Append( Out.Tab( "<div class=\"text\">", "" ) );
            this.mainPage.builder.Append( "〓奖池查询 〓" );
            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            ptype = Utils.ParseInt( Utils.GetRequest( "ptype", "get", 1, @"^[0-1]$", "请求类型错误" ) );
            this.baseMaster.Title = "奖池查询-" + ( ptype == 0 ? "奖池概览" : "奖池明细" );


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


            IncludePage( new SSEPageFooter( this.mainPage, this.baseMaster, this.verMgr ) );
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

                strWhere += string.Format( " and p.sseNo>={0}",  beginTime );
            }


            if( string.IsNullOrEmpty( endTime ) == false )
            {
                strWhere += string.Format( " and p.sseNo<={0}",  endTime );                
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

                    ////获取总奖池 值
                    //decimal sumBeforeOpenPrizeVal = 00dfdfnew BCW.SSE.BLL.SsePrizePoolChang().GetBeforeOpenPrizeVal( n.sseNo );
                    ////获取本期猜涨总额
                    //decimal sumUpVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( n.sseNo, 1 );
                    //decimal sumDownVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( n.sseNo, 0 );

                    //this.mainPage.builder.Append( string.Format( "【{0}】,{1},{2}",n.id.ToString(),n.buyType.ToString(),n.sseNo.ToString()));
                    if( n.sseNo != BCW.SSE.SSE.Instance().GetBuySseNo() )
                        this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">第【{0}】期奖池</a>【{1}】", n.sseNo.ToString(), n.openResult ) );
                        //this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">第【{0}】期奖池</a>【{1}】<b style=\"color:#ff0000\"> | 总奖池：{2} | 猜涨：{3} | 猜跌：{4}</b>", n.sseNo.ToString(), n.openResult, sumBeforeOpenPrizeVal.ToString( "#0" ), sumUpVal.ToString( "#0" ), sumDownVal.ToString( "#0" ) ) );
                    else
                        this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">系统当前奖池<b style=\"color:#ff0000\">(第{0}期)</b></a><b style=\"color:#ff0000\"> | 结余：{1} </b>", n.sseNo.ToString(), n.poolVal.ToString( "#0" ) + this.mSseVersionMgr.account.name ) );
                        //this.mainPage.builder.Append( string.Format( "<a href=\"" + Utils.getUrl(  this.mSseVersionMgr.pageName + "act=prizepool&amp;ptype=1&amp;pSseNo=" + n.sseNo ) + "\">系统当前奖池<b style=\"color:#ff0000\">(第{0}期)</b></a><b style=\"color:#ff0000\"> | 结余：{1} | 猜涨：{2} | 猜跌：{3}</b>", n.sseNo.ToString(), n.poolVal.ToString( "#0.00" ), sumUpVal.ToString( "#0" ), sumDownVal.ToString( "#0" ) ) );
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

            this.mainPage.builder.Append( Out.Tab( "<div>", "" ) );
            this.mainPage.builder.Append( "<br />" );

            this.mainPage.builder.Append( Out.Tab( "</div>", "<br />" ) );

            string strText = "开始期数：,结束期数：,";
            string strName = "sTime,oTime,backurl";
            string strType = "text,text,hidden";
            string strValu = "" + DateTime.Now.AddMonths( -1 ).ToString( "yyyyMMdd" ) + "'" + DateTime.Now.ToString( "yyyyMMdd" ) + "'" + "" + "";

            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "马上查询/," + this.mSseVersionMgr.pageName + "act=prizepool&amp;ptype=" + ptype + ",post,1,red";
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
                decimal sumBeforeOpenPrizeVal = new BCW.SSE.BLL.SsePrizePoolChang().GetBeforeOpenPrizeVal( int.Parse(sseNo) );
                //获取本期猜涨总额
                decimal sumUpVal = new BCW.SSE.BLL.SseOrder().GetGuessMoney( this.mSseVersionMgr.versionId, int.Parse( sseNo ), 1 );
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
                            this.mainPage.builder.Append( string.Format( "{0}、<a href=\"/bbs/uinfo.aspx?uid=" + n.OperId + "\">{1}</a>在第{2}期<b style=\"color:#06A545\">消费{3}{4}</b>|<b style=\"color:#ff0000\">结余：{5}</b>({6})", k + ( ( pageIndex - 1 ) * pageSize ), new BCW.BLL.User().GetUsName( n.OperId ) + "(" + n.OperId + ")", n.sseNo, n.changeMoney.ToString( "#0" ), coinName, n.totalMoney.ToString( "#0" ) + this.mSseVersionMgr.account.name, n.changeTime ) );
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
                this.mainPage.builder.Append( BasePage.MultiPage( pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0 ) );

            }
            else
            {
                this.mainPage.builder.Append( Out.Div( "div", "没有相关记录..." ) );
            }

        }
    }
     #endregion

}