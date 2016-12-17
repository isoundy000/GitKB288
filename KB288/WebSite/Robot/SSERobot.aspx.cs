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
using System.Collections.Generic;
using BCW.Common;
using System.Data.SqlClient;

public partial class Robot_SSERobot : System.Web.UI.Page
{
    private string xmlPath = "/Controls/SSE.xml";

    protected void Page_Load( object sender, EventArgs e )
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds( -1 );
        Response.Expires = 0;
        Response.CacheControl = "no-cache";  


        //机器人自动下游是否开启
        int _robotState =int.Parse(ub.GetSub("SSERobotState",xmlPath));
        if( _robotState == 0 )
        {
            HttpContext.Current.Response.Write( "===========机器人自动投注功能已关闭===========<br />" );
            return;
        }


        //是否在下注时间里
        if( BCW.SSE.SSE.Instance().CheckBuyValidity == false )
        {
            Response.Write( "现在是非下单时间，不能进行自动投注" );
            return;
        }


        RobotStrategyManager.Instance().SetStrategy(ESseRobotStrategyName.e_normal);        //设置执行哪种下单策略
        RobotStrategyManager.Instance().Execute();                                          //执行
        RobotStrategyManager.Instance().currRobot.responseInfo = ResopnseInfo;
    }

    public void ResopnseInfo()
    {
        SSERobot_Normal _robot = ( SSERobot_Normal ) RobotStrategyManager.Instance().currRobot;

        HttpContext.Current.Response.Write( "===========以下是自动投注信息===========<br />" );
        HttpContext.Current.Response.Write( string.Format( "机器人ID：{0} <br />", _robot.meid ) );
        HttpContext.Current.Response.Write( string.Format( "机器人名称：{0} <br />", _robot.self.UsName ) );
        HttpContext.Current.Response.Write( string.Format( "投注期数：{0} <br />", _robot.sseNo ) );
        HttpContext.Current.Response.Write( string.Format( "投注类型：{0} <br />", _robot.buyType == 0 ? "猜跌" : "猜涨" ) );
        HttpContext.Current.Response.Write( string.Format( "投注金额：{0} <br />", _robot.buyMoney ) );
    }
}



public enum ESseOrderType
{
    e_random = 0,       //随机下单
    e_sequence          //顺序下单
}

public enum ESseRobotStrategyName
{
    e_normal,           //总下注策略
}

//机器人基类
public abstract class ISSERobot
{
    public int meid;    
    public BCW.Model.User self;    //自身user对象   
    public delegate void ResponseInfo();      //绑定的显示接口
    public ResponseInfo responseInfo; 


    //机器人初始化
    public virtual void Init(int _meid)
    {
        meid = _meid;     
        self = new BCW.BLL.User().GetBasic(_meid);     

    }  

     
    //机器人下单
    public virtual void SendOrder(){}

    //显示下单信息
    public void ShowOrderInfo()
    {
        if( responseInfo != null )
            responseInfo();
    }
}

//常规机器人
public class SSERobot_Normal:ISSERobot
{
    
    public int sseNo;            //期数 
    public int buyType;          //下单类型(0:跌 1：涨) 
    public int buyMoney;         //下单金额

    public override void SendOrder()
    {
        //执行随机投注  
        int _orderId =  BCW.SSE.SSE.Instance().CreateNewOrder( this.sseNo, this.buyType, this.buyMoney ,meid,0 ,true);
        if( _orderId > 0 )
        {
            //变更奖池.                  
            IDataParameter[] _parameters = new SqlParameter[ 3 ];
            _parameters[ 0 ] = new SqlParameter( "@orderId", SqlDbType.Int );
            _parameters[ 1 ] = new SqlParameter( "@operType", SqlDbType.Int );
            _parameters[ 2 ] = new SqlParameter( "@operUserId", SqlDbType.Int );
            _parameters[ 0 ].Value = _orderId;
            _parameters[ 1 ].Value = 0;             //下注.   
            _parameters[ 2 ].Value = meid;
            BCW.Data.SqlHelper.ExecuteRunProcedure( "sp_SseChangePrizePool", _parameters );

            //写入游戏动态
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName( meid ) + "[/url]在[url=/bbs/game/SSE.aspx]上证指数第" + this.sseNo + "期[/url]下注**" + ub.Get( "SiteBz" ) + "";//" + Convert.ToInt64(allBuyCent) + "
            new BCW.BLL.Action().Add( 1021, _orderId, 0, "", wText );

            //扣币
            new BCW.BLL.User().UpdateiGold( meid, -this.buyMoney, string.Format( "上证|{0}期|标识:{1}|【{2}】", this.sseNo, _orderId, buyType == 1 ? "猜涨" : "猜跌" ) );

            // 更新机器人在线时长
            new BCW.BLL.User().UpdateTime( meid, 5 );
        }
    }

}


#region 自动下注策略
public abstract class ISSERobotStrategy
{
    protected List<ISSERobot> lstRobot;
    protected string xmlPath = "/Controls/SSE.xml";
    protected string[] arryRobot;

    public ISSERobot currRobot; 
    public virtual void Enter(){}
    public abstract void Execute();   //自动下注.
    public virtual void Exit(){}

    public ISSERobotStrategy()
    {
        lstRobot = new List<ISSERobot>();  
        this.Init();
    } 

    //加载机器人.
    private void Init()
    {
        //初始化机器人
        arryRobot = ub.GetSub("SSERobot",xmlPath).Split('#');

    }  
 
    //返回合适下单的机器人
    protected virtual ISSERobot GetRobot(int needMoney)
    {     
        int i=0;
       
        while (i<=lstRobot.Count-1)            //随机抽取满足够钱的机器人.
        {
            Random _ran = new Random();
            int _ranIndex =_ran.Next(0,lstRobot.Count);
            currRobot = lstRobot[ _ranIndex ];
            if( currRobot != null )//&& currRobot.self.iGold >= needMoney )
                return currRobot;
            i++;
        } 
        return null;
    }

}

public class RobotStrategyManager
{
    private static RobotStrategyManager mInstance;
    private Dictionary<ESseRobotStrategyName,ISSERobotStrategy> dctRobotStrategy;
    private ISSERobotStrategy mCurrRobotStrategy;

    public static RobotStrategyManager Instance()
    {
        if( mInstance == null )
            mInstance = new RobotStrategyManager();
        return mInstance;
    }

    public RobotStrategyManager()
    {
        dctRobotStrategy = new Dictionary<ESseRobotStrategyName, ISSERobotStrategy>();
        this.RegisterStrategy();
    }

    private void RegisterStrategy()
    {   
        dctRobotStrategy.Add(ESseRobotStrategyName.e_normal,new SSERobotStrategy_Normal());          //总下注策略         
    }

    public void SetStrategy(ESseRobotStrategyName _strategyName )
    {
       
        if( mCurrRobotStrategy == dctRobotStrategy[ _strategyName ] )
            return;

        if( mCurrRobotStrategy != null )
            mCurrRobotStrategy.Exit();

        mCurrRobotStrategy = dctRobotStrategy[_strategyName];
        if (mCurrRobotStrategy!= null)
            mCurrRobotStrategy.Enter();
    }

    public void Execute()
    {
       mCurrRobotStrategy.Execute();
    }

    public ISSERobot currRobot
    {
        get
        {
            return this.mCurrRobotStrategy.currRobot;
        }
    }
}
   

//总下注策略
//每一期，将总投注量的任务平均分配到随机或顺序的机器人去执行
public class SSERobotStrategy_Normal:ISSERobotStrategy
{
    private float _sendOrderSpace;               //投注时间间隔.
    private DateTime _lastOrderTime;            //上次投注时的时间.
         

    public override void Enter()
    {  
       //创建该策略下对应的机器人
        SSERobot_Normal _robot;

        if( arryRobot != null )
        {
            foreach( string _robotId in arryRobot )
            {
                _robot = new SSERobot_Normal();
                _robot.Init( int.Parse( _robotId ) );
                lstRobot.Add( _robot );
            }
        }

        _sendOrderSpace = Utils.ParseInt( ub.GetSub( "SSERobotOrderSpace", xmlPath ) );

        //计算1个投注周期内每笔自动投注的时间间隔
        _lastOrderTime = System.DateTime.Now;
    }
    
    public override void Execute()
    {
        if( _sendOrderSpace != Utils.ParseInt( ub.GetSub( "SSERobotOrderSpace", xmlPath ) ) )
        {

            _sendOrderSpace = Utils.ParseInt( ub.GetSub( "SSERobotOrderSpace", xmlPath ) );
            _lastOrderTime = System.DateTime.Now;
        }

        //机器人下单
        string[] _lstOrderPrice = ub.GetSub( "SSERobotBPrice", xmlPath ).Split( '#' );

        int _buyMoney = int.Parse(_lstOrderPrice[new Random().Next( 0,_lstOrderPrice.Length )]);

        //随机获取一个符合下单条件的机器人并赋值下单参数;
        this.currRobot = this.GetRobot( _buyMoney );

        //是否到达投注间隔.
        DateTime t1 = System.DateTime.Now;
        TimeSpan _ts = t1.Subtract( _lastOrderTime );
        if( ( _ts.Days * 24 * 3600 + _ts.Hours * 3600 + _ts.Minutes * 60 + _ts.Seconds ) < _sendOrderSpace )
            return;  

        if( currRobot != null )
        {
            ( ( SSERobot_Normal ) currRobot ).buyType = new Random().Next( 0, 2 );               //购买类型.
            ( ( SSERobot_Normal ) currRobot ).buyMoney = _buyMoney;                              //购买随机金额
            ( ( SSERobot_Normal ) currRobot ).sseNo = BCW.SSE.SSE.Instance().GetBuySseNo();      //购买期数  
            currRobot.SendOrder();
            currRobot.ShowOrderInfo();
            _lastOrderTime = System.DateTime.Now;
        }        
    }

    public override void Exit()
    {
        ;
       // throw new NotImplementedException();
    }

    public int sendOrderSpace
    {
        set
        {
            _sendOrderSpace = value;
        }
    }

}


#endregion
