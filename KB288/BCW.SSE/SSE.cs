using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BCW.Common;
using System.Data.SqlClient;



namespace BCW.SSE
{

    public class SSE
    {

        private static SSE mInstance;
        private string xmlPath = "/Controls/SSE.xml";


        public static SSE Instance()
        {
            if (mInstance == null)
                mInstance = new SSE();
            return mInstance;
        }


        
        //判断是否可以下注：
        private bool checkSseBuy()
        {
            DateTime _beginTime = DateTime.Parse( ub.GetSub( "SSEBegin", xmlPath ) );
            DateTime _endTime = DateTime.Parse( ub.GetSub( "SSEEnd", xmlPath ) );


            if( System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime )              //符合条件(但周末始终是休市的)
            {

                return true;
            }
               

            return false;                  

            ////周末不能下注.
            //if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday || System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            //    return false;

            ////是否在限制下注时段.
            //DateTime _beginTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + ub.GetSub("SSEBegin", xmlPath));
            //DateTime _endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + ub.GetSub("SSEEnd", xmlPath));

            //if (System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime)
            //    return false; 

            //return true;
        }


        //计算当期剩余可购买的额度.
        private long GetResidualAmount()
        {
            int _sseNo = this.GetBuySseNo();
            long _residualAmount = long.Parse(ub.GetSub("SSEBig", xmlPath));            //本期可购总额度.

            //查询已经购买的额度.
            DataSet _dataSet = BCW.Data.SqlHelper.Query(string.Format("select sseNo,SUM(buyMoney)buyMoney from tb_SseOrder where sseNo = {0} and userId ={1} and orderType = 0 group by sseNo order by sseNo", _sseNo.ToString(),new BCW.User.Users().GetUsId()));
            if (_dataSet.Tables[0].Rows.Count > 0)
                _residualAmount -= Convert.ToInt64(_dataSet.Tables[0].Rows[0]["buyMoney"]);

            return _residualAmount;
        }
             

        //返回以日期为组合的期数编号.
        public int GetBuySseNo()
        {
            DateTime _openTime = DateTime.Parse(ub.GetSub( "SSEOpen", xmlPath ));
            return int.Parse( _openTime.Year.ToString() + _openTime.Month.ToString( "#00" ) + _openTime.Day.ToString( "#00" ) );   
                         

            // int _today = int.Parse(System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString("#00") + System.DateTime.Now.Day.ToString("#00"));

            // DateTime _newDate = System.DateTime.Now;

            ////当天未开将.
            // DataSet _dataSet = new BLL.SseBase().GetList(1, string.Format("sseNo={0}",_today), "sseNo");
            // if (_dataSet.Tables[0].Rows.Count <= 0)    
            //    return _today;
            // else
            // {
            //     //跳过星期六日.                 
            //     if (System.DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            //         _newDate = _newDate.AddDays(3);
            //     else if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            //         _newDate = _newDate.AddDays(2);
            //     else
            //         _newDate = _newDate.AddDays(1);
            //  }
 

            //return int.Parse(_newDate.Year.ToString() + _newDate.Month.ToString("#00") + _newDate.Day.ToString("#00"));    
  
           
        }

        //获取昨收价.
        public double GetYesterdayPrice()
        {
            DataSet _dataSet =  new BLL.SseBase().GetList(1, "1=1", "sseNo desc");
            if (_dataSet.Tables[0].Rows.Count >0)
             return Convert.ToDouble(_dataSet.Tables[0].Rows[0]["closePrice"].ToString());
            return 0;
        }

        //确认下注.
        public int CreateNewOrder(int orderType,int sseNo,int buyType, Int64 money, bool isAutoOrder)
        {
            int id;
            int _orderType = orderType;         //酷币or金币订单.
            int _sseNo = sseNo;         //期数.
            DateTime _buyTime = System.DateTime.Now;
            int _userId = new BCW.User.Users().GetUsId();
            int _buyType = buyType;
            Int64 _money = money;

            BCW.SSE.Model.SseOrder _sseOrderMod = new BCW.SSE.Model.SseOrder();
            _sseOrderMod.orderType = _orderType;
            _sseOrderMod.sseNo = _sseNo;
            _sseOrderMod.orderDateTime = _buyTime;
            _sseOrderMod.userId = _userId;
            _sseOrderMod.buyType = Convert.ToBoolean(buyType);
            _sseOrderMod.buyMoney = _money;
            _sseOrderMod.state = 0;
            _sseOrderMod.bz = "";
            _sseOrderMod.isAutoOrder = isAutoOrder;

            id = new BCW.SSE.BLL.SseOrder().Add(_sseOrderMod);
            return  id;

        }

        //确认下注.
        public int CreateNewOrder( int sseNo, int buyType, Int64 money ,int userId,int orderType, bool isAutoOrder)
        {
            int id;
            int _orderType = orderType;         //酷币订单.
            int _sseNo = sseNo;         //期数.
            DateTime _buyTime = System.DateTime.Now;
            int _userId = userId;
            int _buyType = buyType;
            Int64 _money = money;

            BCW.SSE.Model.SseOrder _sseOrderMod = new BCW.SSE.Model.SseOrder();
            _sseOrderMod.orderType = _orderType;
            _sseOrderMod.sseNo = _sseNo;
            _sseOrderMod.orderDateTime = _buyTime;
            _sseOrderMod.userId = _userId;
            _sseOrderMod.buyType = Convert.ToBoolean( buyType );
            _sseOrderMod.buyMoney = _money;
            _sseOrderMod.state = 0;
            _sseOrderMod.bz = "";
            _sseOrderMod.isAutoOrder = isAutoOrder;

            id = new BCW.SSE.BLL.SseOrder().Add( _sseOrderMod );
            return id;

        }

        //确认退注.


        //确认兑奖(返回订单ID).
        public int GetPrize( int _prizeNo, long _money )
        {
            string _updateSqlStr = string.Format( "update tb_SseGetPrize set isGet=1,getDateTime='{0}' where id = {1}", System.DateTime.Now, _prizeNo );
            BCW.Data.SqlHelper.ExecuteSql( _updateSqlStr );

            string _querySqlStr = "select orderId from tb_SseGetPrize where id=" + _prizeNo.ToString();
            DataSet _ds = BCW.Data.SqlHelper.Query( _querySqlStr );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {
                return int.Parse(_ds.Tables[ 0 ].Rows[ 0 ][ "orderId" ].ToString());
            }

            return 0;
        }


        #region 获取页脚统计信息
        public List<string> GetFooterData(int _versionId)
        {
            List<string> _result = new List<string>();

            //统计当期投注总额.

            DataSet _dsPrizePool = BCW.Data.SqlHelper.Query( string.Format( "select  top 1 totalPrizeMoney from tb_SsePrizePool where sseNo = {0} and prizeType={1}", BCW.SSE.SSE.Instance().GetBuySseNo(), _versionId ) );
            //string _prizePoolVal = long.Parse( _dsPrizePool.Tables[ 0 ].Rows.Count == 0 ? "0" : _dsPrizePool.Tables[ 0 ].Rows[ 0 ][ "totalPrizeMoney" ].ToString() ).ToString();


            DataSet _dsOrder = BCW.Data.SqlHelper.Query(string.Format("select SUM(buyMoney)buyMoney from tb_SseOrder where sseNo = {0} and orderType = {1} and state=0", BCW.SSE.SSE.Instance().GetBuySseNo(),_versionId));
            _result.Add( ( _dsOrder.Tables[ 0 ].Rows.Count == 0 ? "0" : _dsOrder.Tables[ 0 ].Rows[ 0 ][ "buyMoney" ].ToString() ));

            DataSet _ds1 = BCW.Data.SqlHelper.Query( string.Format( "select SUM(buyMoney) buyMoney from tb_SseOrder where sseNo = {0} and orderType = {1} and buyType = 1 and state=0 group by buyType", BCW.SSE.SSE.Instance().GetBuySseNo() ,_versionId) );
            _result.Add(_ds1.Tables[0].Rows.Count == 0 ? "0" : _ds1.Tables[0].Rows[0]["buyMoney"].ToString());

            DataSet _ds2 = BCW.Data.SqlHelper.Query( string.Format( "select SUM(buyMoney) buyMoney from tb_SseOrder where sseNo = {0} and orderType = {1} and buyType = 0 and state=0 group by buyType", BCW.SSE.SSE.Instance().GetBuySseNo(),_versionId ) );
            _result.Add(_ds2.Tables[0].Rows.Count == 0 ? "0" :_ds2.Tables[0].Rows[0]["buyMoney"].ToString());

            _result.Add( _dsPrizePool.Tables[ 0 ].Rows.Count == 0 ? "0" : _dsPrizePool.Tables[ 0 ].Rows[ 0 ][ "totalPrizeMoney" ].ToString() );

            return _result;   


            //_lstResult.Append      

            //DataSet ds2 = BCW.Data.SqlHelper.Query(string.Format("select COUNT(*)count,SUM(buyMoney) buyMoney,buyType from tb_SseOrder where sseNo = {0} and orderType = 0 group by buyType order by buyType desc", BCW.SSE.SSE.Instance().GetBuySseNo()));
                         
            //return null;
        }

        #endregion

        #region   计算截止投注时间
        public string GetEndOrderTime()
        {

            if( CheckBuyValidity == false)
                return "已截止投注";

            //计算截止下注时间.
            System.DateTime _endTime = DateTime.Parse( ub.GetSub( "SSEEnd", xmlPath ) );     //开始截止下注的时间.

            //如果是周五，则周五截止时间到00：00：00
            //if( DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            //    _endTime = Convert.ToDateTime( DateTime.Now.ToString( "yyyy-MM-dd" ) + " 23:59:59" );


            string _countDownTim = "";
            if( Utils.Isie() || Utils.GetUA().ToLower().Contains( "opera/8" ) )
            {
                _countDownTim = new BCW.JS.somejs().daojishi( "divSse", _endTime );
            }
            else
            {
                _countDownTim = new BCW.JS.somejs().daojishi2( "divSse", Convert.ToDateTime( _endTime ) );
            }
            return string.Format( "离本期截止投注时间还有{0}", _countDownTim );
        }

        #endregion 


        public bool CheckBuyValidity 
        {
            get {return checkSseBuy();}
        }

        public long residualAmount
        {
            get { return this.GetResidualAmount(); }
        }


    }


    #region   后台管理
    public class SSEManage
    {
        private static SSEManage mInstance;
        private string xmlPath = "/Controls/SSE.xml";


        public static SSEManage Instance()
        {
            if (mInstance == null)
                mInstance = new SSEManage();
            return mInstance;
        }

        //获取用记投注订单信息(分页)
        public IList<SseManageClass.ManagerOrder> GetSseOrderPages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<SseManageClass.ManagerOrder> listSseOrder = new List<SseManageClass.ManagerOrder>();

            // 取出相关记录
            string queryString = "";

            // 计算记录数
            string countString = "select COUNT(*) from (select a.* ,(case when b.isGet is null then '未中奖'	when b.isGet=0 then '未兑奖' when b.isGet=1 then '已兑奖' when b.openDateTime <= DATEADD(MM,-1,GETDATE()) then '已过期' end)as oState from tb_SseOrder a left join ve_SseGetPrize b on a.id = b.orderId  where state = 0 ) b where state = 0 and 1=1 ".Replace( "1=1", strWhere );

            p_recordCount = Convert.ToInt32(BCW.Data.SqlHelper.GetSingle( countString ) );

            if( p_recordCount > 0 )
            {
                int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
            }
            else
            {
                return listSseOrder;
            }


            queryString = "select * from (select a.*,b.orderId,b.isGet,b.openDateTime,b.prizeVal,(case when b.isGet is null then '未中奖'	when b.isGet=0 then '未兑奖' when b.isGet=1 then '已兑奖' when b.openDateTime <= DATEADD(MM,-1,GETDATE()) then '已过期' end)as oState from tb_SseOrder a left join ve_SseGetPrize b on a.id = b.orderId)aa where 1=1 and state = 0  order by id desc ".Replace( "1=1", strWhere );
            using( SqlDataReader reader = BCW.Data.SqlHelper.ExecuteReader( queryString ) )
            {
                int stratIndex = ( p_pageIndex - 1 ) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                             
                while( reader.Read() )                    
                {
                    if( k >= stratIndex && k < endIndex )
                    {
                        SseManageClass.ManagerOrder objOrder = new SseManageClass.ManagerOrder();

                        objOrder.id = reader.GetInt32( 0 );
                        objOrder.orderType = reader.GetByte(1) ;
                        objOrder.sseNo =  reader.GetInt32( 2 );
                        objOrder.orderDateTime = reader.GetDateTime( 3 );
                        objOrder.userId = reader.GetInt32( 4 );
                        objOrder.buyType = reader.GetBoolean( 5 );
                        objOrder.buyMoney = reader.GetDecimal( 6 );
                        if (Convert.IsDBNull( reader[11]) == false )
                        {
                            objOrder.isGet =reader.GetBoolean( 11 ) ;
                            objOrder.openDateTime = reader.GetDateTime( 12 );
                            objOrder.prizeVal = reader.GetDecimal( 13 );                              
                        }
                        objOrder.state = reader.GetString( 14 );

                        listSseOrder.Add( objOrder );
                    }
                    if( k == endIndex )
                        break;

                    k++;
                }
            }

            return listSseOrder;

            //return dal.GetSseOrderPages( p_pageIndex, p_pageSize, strWhere, out p_recordCount );
        }

        //获取奖池概览(分页)
        public IList<SseManageClass.ManagerPoolOveriew> GetPoolOveriew( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<SseManageClass.ManagerPoolOveriew> listSseOrder = new List<SseManageClass.ManagerPoolOveriew>();

            // 取出相关记录
            string queryString = "";

            // 计算记录数
            string countString = "select COUNT(*)from tb_SsePrizePool p where 1=1 ".Replace( "1=1", strWhere );

            p_recordCount = Convert.ToInt32( BCW.Data.SqlHelper.GetSingle( countString ) );

            if( p_recordCount > 0 )
            {
                int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
            }
            else
            {
                return listSseOrder;
            }


            queryString = " select p.sseNo,p.totalPrizeMoney,case b.bz when '1' then '涨' when '0' then '跌' when '-1' then '平' else '-' end openResult  from tb_SsePrizePool p left join tb_SseBase b on p.sseNo = b.sseNo where 1=1 order by p.sseNo desc".Replace( "1=1", strWhere );
            using( SqlDataReader reader = BCW.Data.SqlHelper.ExecuteReader( queryString ) )
            {
                int stratIndex = ( p_pageIndex - 1 ) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while( reader.Read() )
                {
                    if( k >= stratIndex && k < endIndex )
                    {
                        SseManageClass.ManagerPoolOveriew objPoolOveriew = new SseManageClass.ManagerPoolOveriew();

                        objPoolOveriew.sseNo = reader.GetInt32( 0 );
                        objPoolOveriew.poolVal = reader.GetDecimal( 1 );
                        objPoolOveriew.openResult = reader.GetString( 2 );

                        listSseOrder.Add( objPoolOveriew );
                    }
                    if( k == endIndex )
                        break;

                    k++;
                }
            }

            return listSseOrder;
            //return dal.GetSseOrderPages( p_pageIndex, p_pageSize, strWhere, out p_recordCount );
        }

        //获取奖池(分页)
        public IList<SseManageClass.ManagerPoolDetial> GetPoolDetial( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<SseManageClass.ManagerPoolDetial> listSseOrder = new List<SseManageClass.ManagerPoolDetial>();

            // 取出相关记录
            string queryString = "";

            // 计算记录数
            string countString = "select COUNT(*) from tb_SsePrizePoolChang  where poolType = 0 and 1=1 ".Replace( "1=1", strWhere );
            
            p_recordCount = Convert.ToInt32( BCW.Data.SqlHelper.GetSingle( countString ) );

            if( p_recordCount > 0 )
            {
                int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
            }
            else
            {
                return listSseOrder;
            }


            queryString = "select poolType,operType,changeTime,changeMoney,totalMoney,bz,sseNo from tb_SsePrizePoolChang where poolType = 0 and 1=1 order by changeTime desc".Replace( "1=1", strWhere );
            using( SqlDataReader reader = BCW.Data.SqlHelper.ExecuteReader( queryString ) )
            {
                int stratIndex = ( p_pageIndex - 1 ) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while( reader.Read() )
                {
                    if( k >= stratIndex && k < endIndex )
                    {
                        SseManageClass.ManagerPoolDetial objPoolOveriew = new SseManageClass.ManagerPoolDetial();

                        objPoolOveriew.poolType = reader.GetInt32( 0 );
                        objPoolOveriew.operType = reader.GetInt32( 1 );
                        objPoolOveriew.changeTime = reader.GetDateTime( 2 );
                        objPoolOveriew.changeMoney = reader.GetDecimal( 3 );
                        objPoolOveriew.totalMoney = reader.GetDecimal( 4 );
                        objPoolOveriew.bz = reader.GetString( 5 );
                        objPoolOveriew.sseNo = reader.GetInt32( 6 );

                        listSseOrder.Add( objPoolOveriew );
                    }
                    if( k == endIndex )
                        break;

                    k++;
                }
            }

            return listSseOrder;
            //return dal.GetSseOrderPages( p_pageIndex, p_pageSize, strWhere, out p_recordCount );
        }

    }
    #endregion

   
    public class SSEDataParseNew
    {
        private bool mIsOpenPrize = false;
        public bool autoOpenPrize = true;      //默认自动开奖          
        private string xmlPath;
        public string closePrice = "0";         



        //查询是否已抓取当天的收盘数据

        public bool GetNewSSEData()
        {


            this.xmlPath = "/Controls/SSE.xml";

            DateTime _openPrizeTime = DateTime.Parse(ub.GetSub("SSEOpen",xmlPath));

            //周末休市.
            if (System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday || System.DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                return false;
              
            //自动开奖，开奖时间不能早于设定的开奖时间
            if( autoOpenPrize == true && System.DateTime.Now < _openPrizeTime )
                return false;
                       

            BCW.SSE.BLL.SseBase _sseBase;


            //获取上次的记录.

            decimal _lastPrice = 0;

            //获取上一个开奖日数据
            _sseBase = new BCW.SSE.BLL.SseBase();
            DataSet _dsLastRecord = _sseBase.GetList(1, "", "sseNo desc");
            if (_dsLastRecord.Tables[0].Rows.Count > 0)
            {
                _lastPrice = decimal.Parse(_dsLastRecord.Tables[0].Rows[0]["closePrice"].ToString());
            }

            //开奖的期号

            string _sseNo = System.DateTime.Now.Year.ToString( "#0000" ) + System.DateTime.Now.Month.ToString( "#00" ) + System.DateTime.Now.Day.ToString( "#00" );

            _sseBase = new BCW.SSE.BLL.SseBase();
            DataSet _dataSet = _sseBase.GetList( string.Format( "sseNo={0}", _sseNo ) );

            if (_dataSet.Tables[0].Rows.Count <= 0)
            {
                mIsOpenPrize = true;                 //正在开奖.

                //获取当天收盘价（调用新浪接口取数据）.
                string _closePrice;
                if( closePrice =="0" )
                    _closePrice = new BCW.Service.GetStk().GetStkXMLFormat();
                else
                    _closePrice = closePrice;
                BCW.SSE.Model.SseBase _sseModel = new BCW.SSE.Model.SseBase();
                _sseModel.sseNo = int.Parse( _sseNo );
                _sseModel.closePrice = decimal.Parse(_closePrice);
                _sseModel.bz = _sseModel.closePrice > _lastPrice ? "1" : _sseModel.closePrice < _lastPrice ? "0":"-1";     //1：涨  0：跌  -1：平
                _sseBase.Add(_sseModel);

                //开奖处理.
                //开出平局，处理退注. TODO by zhc 暂时用下面的循环先，数据量大时这种方式效率极差，日后改用数据批量更新.
                if( _sseModel.bz == "-1" )
                {
                    DataSet ds = BCW.Data.SqlHelper.Query( string.Format( "select * from tb_SseOrder where sseNo={0} and state=0", _sseNo ) );  

                    if( ds != null && ds.Tables[ 0 ].Rows.Count > 0 )
                    {

                        for( int i = 0; i < ds.Tables[ 0 ].Rows.Count; i++ )
                        {

                            //退还相应的币种. 
                            int _orderId = int.Parse( ds.Tables[ 0 ].Rows[ i ][ "id" ].ToString() );             //订单用户ID.
                            int _userId = int.Parse( ds.Tables[ 0 ].Rows[ i ][ "userId" ].ToString() );             //订单用户ID.
                            int _moneyType =   int.Parse( ds.Tables[ 0 ].Rows[ i ][ "orderType" ].ToString() );     //订单货币类型.    
                            Int64 _backMoney = (Int64)decimal.Parse( ds.Tables[ 0 ].Rows[ i ][ "buyMoney" ].ToString() );     //订单金额.


                            //变更订单状态
                            BCW.SSE.Model.SseOrder modelSseOrder = new BCW.SSE.BLL.SseOrder().GetModel( _orderId  );
                            modelSseOrder.bz = "2";                                    //2：特殊处理;记录一下订单是系统平局退回
                            new BCW.SSE.BLL.SseOrder().Update( modelSseOrder );


                            //退还酷币
                            string strLog = "";
                            if( _moneyType == 0 )
                            {
                                new BCW.BLL.User().UpdateiGold( _userId, ( long ) _backMoney, string.Format( "上证【{0}】期平局退回|标识({1})", _sseNo, _orderId ) );                 
                                strLog = string.Format( "上证【{0}】期平局退回|金额{1})，请注意查收", _sseNo, _backMoney.ToString( "0" ) + ub.Get( "SiteBz" ) );
                            }
                            //退还农场金币
                            else
                            {
                                new BCW.farm.BLL.NC_user().UpdateiGold( _userId, new BCW.BLL.User().GetUsName( _userId ), ( long ) _backMoney, string.Format( "上证【{0}】期平局退回|标识({1})|获得{2}金币|结{3}金币)", _sseNo, _orderId, Math.Abs( _backMoney ), (new BCW.farm.BLL.NC_user().GetGold( _userId ) + _backMoney).ToString("#0") ), 8 );
                                strLog = string.Format( "上证【{0}】期平局退回|金额{1})，请注意查收", _sseNo, _backMoney.ToString( "0" ) + "金币" );
                            }

                            //发内线
                            if( string.IsNullOrEmpty( strLog )==false )
                             new BCW.BLL.Guest().Add( 0, _userId, new BCW.BLL.User().GetUsName( _userId ), strLog ); 
    

                            //更新奖池.               
                            IDataParameter[] _parameters = new SqlParameter[3];
                            _parameters[0] = new SqlParameter("@orderId", SqlDbType.Int);
                            _parameters[1] = new SqlParameter("@operType", SqlDbType.Int);
                            _parameters[2] = new SqlParameter("@operUserId", SqlDbType.Int);  
                            _parameters[0].Value = _orderId;            //订单ID.
                            _parameters[1].Value = 1;                   //系统退回       
                            _parameters[2].Value = -1;                  //系统id为-1
                            BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseChangePrizePool", _parameters );  
                        }
                    }                   
                }


               //如果正常涨跌，交由存储过程处理相关开奖逻辑   

                //酷币开奖.
                IDataParameter[] _parameters2 = new SqlParameter[4];
                _parameters2[ 0 ] = new SqlParameter( "@sseNo", SqlDbType.Int );
                _parameters2[ 1 ] = new SqlParameter( "@prizeCalcRate", SqlDbType.Float );
                _parameters2[ 2 ] = new SqlParameter( "@poundageRate", SqlDbType.Float );
                _parameters2[ 3 ] = new SqlParameter( "@orderType", SqlDbType.Int );
                _parameters2[ 0 ].Value = _sseNo;            //订单ID.                   
                _parameters2[ 1 ].Value = ub.GetSub( "SSEPrizeCalcRate", xmlPath );            //订单计奖率.
                _parameters2[ 2 ].Value = ub.GetSub( "SSEPrizePoundageRate", xmlPath );         //订单兑奖手续费率.
                _parameters2[ 3 ].Value = 0;                                                 
                BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseOpenPrize", _parameters2 );


                //金币开奖.
                _parameters2[ 0 ] = new SqlParameter( "@sseNo", SqlDbType.Int );
                _parameters2[ 1 ] = new SqlParameter( "@prizeCalcRate", SqlDbType.Float );
                _parameters2[ 2 ] = new SqlParameter( "@poundageRate", SqlDbType.Float );
                _parameters2[ 3 ] = new SqlParameter( "@orderType", SqlDbType.Int );
                _parameters2[ 0 ].Value = _sseNo;            //订单ID.                   
                _parameters2[ 1 ].Value = ub.GetSub( "SSEPrizeCalcRate", xmlPath );            //订单计奖率.
                _parameters2[ 2 ].Value = ub.GetSub( "SSEPrizePoundageRate", xmlPath );         //订单兑奖手续费率.
                _parameters2[ 3 ].Value = 1;
                BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseOpenPrize", _parameters2 ); 



                //内线通知中奖玩家
                DataSet _dsGetPrizeUser = new BCW.SSE.BLL.SseGetPrize().GetList( "sseNo ="+_sseNo );
                for( int i=0; i < _dsGetPrizeUser.Tables[ 0 ].Rows.Count;i++ )
                {
                    //发内线
                    //退还酷币
                    string strLog = "";
                    string getStr = "";
                    if( int.Parse(_dsGetPrizeUser.Tables[ 0 ].Rows[ i ][ "orderType" ].ToString()) == 0 )
                    {
                        getStr = "[url=/bbs/game/SSE.aspx?sseVe=0&amp;act=getPirze]马上兑奖[/URL]";
                        strLog = string.Format( "恭喜您在上证第{0}期赢得{1}", _sseNo, decimal.Parse( _dsGetPrizeUser.Tables[ 0 ].Rows[ i ][ "prizeVal" ].ToString() ).ToString( "#0" ) + ub.Get( "SiteBz" ) );
                    }
                    //退还农场金币
                    else
                    {
                        getStr = "[url=/bbs/game/SSE.aspx?sseVe=1&amp;act=getPirze]马上兑奖[/URL]";
                        strLog = string.Format( "恭喜您在上证第{0}期赢得{1}", _sseNo, decimal.Parse( _dsGetPrizeUser.Tables[ 0 ].Rows[ i ][ "prizeVal" ].ToString() ).ToString( "#0" ) + "金币" );
                    }   
                     
                    
                    new BCW.BLL.Guest().Add( 0, int.Parse( _dsGetPrizeUser.Tables[ 0 ].Rows[ i ][ "userId" ].ToString() ), new BCW.BLL.User().GetUsName( int.Parse( _dsGetPrizeUser.Tables[ 0 ].Rows[ i ][ "userId" ].ToString() ) ), strLog + getStr ); 
                }


                mIsOpenPrize = false;
                return true;
            }
            return false;

        }


        public bool isOpeningPrize
        {
            get
            {
                return this.mIsOpenPrize;
            }
        }

    }
}
