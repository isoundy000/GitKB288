using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile.Home;

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

    public class NoticeData : HomeBaseInfo
    {
        public int id;                  //ID
        public string title;           //标题
        public string content;         //内容
        public DateTime addTime;        //发布时间
        public DateTime expireTime;     //过期时间
        public int usId;                //发布者ID
        public string usName;           //发布者名称

        //简要信息(不带内容)
        public override string OutPutJsonStr()
        {
            jsonBuilder.Append( "{" );
            jsonBuilder.Append( string.Format( "\"id\":{0},", this.id ) );
            jsonBuilder.Append( string.Format( "\"userId\":{0},", this.usId ) );
            jsonBuilder.Append( string.Format( "\"nickname\":\"{0}\",", this.usName ) );
            jsonBuilder.Append( string.Format( "\"title\":\"{0}\",", this.title ) );
            jsonBuilder.Append( string.Format( "\"content\":\"{0}\",", this.content ) );
            jsonBuilder.Append( string.Format( "\"posttime\":{0},", this.addTime.Ticks ) );
            jsonBuilder.Append( string.Format( "\"expire\":{0}", this.expireTime.Ticks ) );
            jsonBuilder.Append( "}" );

            return jsonBuilder.ToString();
        }
    }

    public class NoticesBase
    {
        protected bool mFinish;
        public List<NoticeData> lstNoticesItem;   //公告或喇叭信息
        protected string strWhere;
        protected string strSql;


        public NoticesBase()
        {
            lstNoticesItem = new List<NoticeData>();
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
                    _noticeData.title = _ds.Tables[ 0 ].Rows[ i ][ "Title" ].ToString().Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" );
                    string _str = _ds.Tables[ 0 ].Rows[ i ][ "Content" ].ToString();
                    _noticeData.content = Out.SysUBB( _str ).Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
                    _noticeData.usId = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "UsID" ].ToString() );
                    _noticeData.usName = _ds.Tables[ 0 ].Rows[ i ][ "UsName" ].ToString();
                    _noticeData.addTime = DateTime.Parse(_ds.Tables[ 0 ].Rows[ i ][ "addTime" ].ToString());
                    _noticeData.expireTime = DateTime.Parse(_ds.Tables[ 0 ].Rows[ i ][ "OverTime" ].ToString());

                    lstNoticesItem.Add( _noticeData );

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
    public class Notices : HomeBaseInfo
    {
        public bool finish;                 //是否到底
        public DateTime serverTime;             //服务器当前时间 
        public ENoticeType noticeType;      
        public NoticesBase noticesItem;   //数据对象


        public Notices( ENoticeType _type )
        {
            noticeType = _type;
            noticesItem = new NoticesBase();
            switch( _type )
            {
                case ENoticeType.e_all:
                    noticesItem = new NoticesAllItem();
                    break;
                case ENoticeType.e_suona:
                    noticesItem = new NoticesSuonaItem();
                    break;
            }
        }


        private string GetLstNoticesItemStr()
        {
            string _str = "";
            foreach( NoticeData _item in noticesItem.lstNoticesItem )
                _str += _item.OutPutJsonStr() + ",";

            if( _str != "" )
                _str = _str.Substring( 0, _str.Length - 1 );

            return _str;
        }

        public override string OutPutJsonStr()
        {
            jsonBuilder.Append( "\"notices\":{" );
            jsonBuilder.Append( "\"finish\":" + this.finish.ToString().ToLower() + "," );
            jsonBuilder.Append( "\"serverTime\":" + DateTime.Now.Ticks.ToString() + "," );  
            jsonBuilder.Append( "\"items\":[" + this.GetLstNoticesItemStr() );
            jsonBuilder.Append( "]}" );
            return jsonBuilder.ToString();
        }
    }
}
