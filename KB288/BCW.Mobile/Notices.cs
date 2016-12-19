using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BCW.Mobile
{

    /// <summary>
    /// 公告或喇叭类型
    /// </summary>
    public enum ENoticeType
    {
        e_all=0,    //公告和未过期喇叭混合数据  
        e_suona,  //所有喇叭数据(包含已过期的)
    }

    public class NoticeData
    {
        public int id;                  //ID
        public string title;           //标题
        public string content;         //内容
        public long posttime;      //发布时间
        public long expire;        //过期时间
        public int userId;                //发布者ID
        public string nickname;           //发布者名称     
    }

    public class NoticesBase
    {
        protected bool mFinish;
        public List<NoticeData> items;   //公告或喇叭信息
        protected string strWhere;
        protected string strSql;


        public NoticesBase()
        {
            items = new List<NoticeData>();
            strWhere = "";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void InitData()
        {
           
            this.mFinish = true;
            DataSet _ds = BCW.Data.SqlHelper.Query( strSql + strWhere );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {
                for( int i = 0; i < _ds.Tables[ 0 ].Rows.Count; i++ )
                {
                    NoticeData _noticeData = new NoticeData();
                    _noticeData.id = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ID" ].ToString() );
                    _noticeData.title = _ds.Tables[0].Rows[i]["Title"].ToString();//.Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" );
                    string _str = _ds.Tables[ 0 ].Rows[ i ][ "Content" ].ToString();
                    _noticeData.content = Out.SysUBB(_str);//.Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
                    _noticeData.userId = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "UsID" ].ToString() );
                    _noticeData.nickname = _ds.Tables[ 0 ].Rows[ i ][ "UsName" ].ToString(); 
                    System.DateTime _startTime = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime( 1970, 1, 1 ) );
                    _noticeData.posttime = ( long ) ( DateTime.Parse( _ds.Tables[ 0 ].Rows[ i ][ "addTime" ].ToString() ) - _startTime ).TotalSeconds;  
                    _noticeData.expire =( long ) ( DateTime.Parse( _ds.Tables[ 0 ].Rows[ i ][ "OverTime" ].ToString() ) - _startTime ).TotalSeconds;

                    items.Add( _noticeData );

                    //检查是否到底
                    if( i == _ds.Tables[ 0 ].Rows.Count - 1 )
                    {
                        if( strWhere.Contains( "1=1" ) == false )
                            strWhere += " and 1=1";
                        DataSet _dsCheck = BCW.Data.SqlHelper.Query( strSql + strWhere.Replace( "1=1", "ID>" + _noticeData.id ) );
                        mFinish = _dsCheck.Tables[ 0 ].Rows.Count <= 0;
                    }
                }
            }


        }


        /// <summary>
        /// 是否到底
        /// </summary>
        public bool finish
        {
            get
            {
                return this.mFinish;
            }
        }
    }


    /// <summary>
    /// 公告或喇叭内容
    /// </summary>
    public class NoticesAllItem : NoticesBase
    {          
        public void InitData( int noticeId )
        {

            if( noticeId > 0 )
                strWhere += "and ID>" + noticeId;

            strSql = "select * from (" +
                        "select TOP 10 row_number() OVER (order by addTime desc) ID,* from (" +
                             "select Title,Content,addTime,'' OverTime,0 UsID,'系统公告' UsName from  tb_Detail where NodeId=270 union  all " +
                             "select '',Title Content,addTime,OverTime,UsID,UsName from tb_network) notices" +
                      ")notices where 2=2 ";

            base.InitData( );
        }
    }

    /// <summary>
    /// 喇叭内容
    /// </summary>
    public class NoticesSuonaItem : NoticesBase
    {

        public void InitData( int _noticeId, int _expire,int _usId )
        {

            if( _noticeId > 0 )
                strWhere += "and ID>" + _noticeId;
            if( _expire > 0 )
                strWhere += " and GETDATE()>OverTime";
            if( _usId >0)
                strWhere += " and UsID=" + _usId;

            strSql = "select * from (" + 
                    "select row_number() OVER (order by addTime desc) ID,* from ( " +
                    "select Title,'' Content,addTime,OverTime,UsID,UsName from tb_network) notices" +
                    ")notices where 2=2 ";

            base.InitData( );

        }         
    }

    /// <summary>
    /// 公告或喇叭正文
    /// </summary>
    public class Notices
    {
        public long serverTime;         //服务器当前时间 
        [JsonIgnore]
        public ENoticeType noticeType;
        public NoticesBase noticeData;      //数据对象


        public Notices( ENoticeType _type )
        {
            noticeType = _type;
            noticeData = new NoticesBase();
            switch( _type )
            {
                case ENoticeType.e_all:
                    noticeData = new NoticesAllItem();
                    break;
                case ENoticeType.e_suona:
                    noticeData = new NoticesSuonaItem();
                    break;
            }
            System.DateTime _startTime = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime( 1970, 1, 1 ) );
            serverTime = ( long ) ( DateTime.Now - _startTime ).TotalSeconds;

        } 

        //
    }
}
