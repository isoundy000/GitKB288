using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Protocol;
using System.Text.RegularExpressions;

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

            strSql = "select top 10 * from (" +
                        "select  row_number() OVER (order by addTime desc) ID,* from (" +
                             "select Title,Content,addTime,'' OverTime,0 UsID,'系统公告' UsName from  tb_Detail where NodeId=270 union  all " +
                             "select '',Title Content,addTime,OverTime,UsID,UsName from tb_network where GETDATE()<OverTime) notices" +
                      ")notices where 2=2";

            base.InitData( );
        }
    }

    /// <summary>
    /// 喇叭内容
    /// </summary>
    public class NoticesSuonaItem : NoticesBase
    {
        protected string xmlPath = "/Controls/network.xml";

        public void InitData( int _noticeId, int _expire,int _usId )
        {

            if( _noticeId > 0 )
                strWhere += "and ID>" + _noticeId;
            if( _expire > 0 )
                strWhere += " and GETDATE()>OverTime";
            else if (_expire ==0 )
                strWhere += " and GETDATE()<=OverTime";

            if ( _usId >0)
                strWhere += " and UsID=" + _usId;

            strSql = "select top 10 * from (" + 
                    "select row_number() OVER (order by addTime desc) ID,* from ( " +
                    "select Title,'' Content,addTime,OverTime,UsID,UsName from tb_network) notices" +
                    ")notices where 2=2 ";

            base.InitData( );

        }

        public RspAddSuona AddSuonaItem(ReqAddSuona _reqData)
        {
            RspAddSuona _rspAddSuona = new RspAddSuona();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspAddSuona;
            }

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspAddSuona;
            }
            
            int RegDay = Utils.ParseInt(ub.GetSub("NetworkRegDay", xmlPath));
            int Grade = Utils.ParseInt(ub.GetSub("NetworkGrade", xmlPath));
            if (RegDay > 0 || Grade > 0)
            {
                DataSet ds = new BCW.BLL.User().GetList("RegTime,Leven", "id=" + _reqData.userId + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DateTime RegTime = DateTime.Parse(ds.Tables[0].Rows[0]["RegTime"].ToString());
                    int Leven = int.Parse(ds.Tables[0].Rows[0]["Leven"].ToString());
                    if (RegDay > 0 && RegTime > DateTime.Now.AddDays(-RegDay))
                    {
                        _rspAddSuona.header.status = ERequestResult.faild;
                        _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.NETWORK_SUONA_REGDAY_NOT_ENOUGH;
                        return _rspAddSuona;
                    }

                    if (Grade > 0 && Leven < Grade)
                    {
                        _rspAddSuona.header.status = ERequestResult.faild;
                        _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.NETWORK_SUONA_LEVEL_NOT_ENOUGH;
                        return _rspAddSuona;  
                    }
                }
            }

            //自身权限不足
            if (new BCW.User.Limits().IsUserLimit(User.Limits.enumRole.Role_NetWork, _reqData.userId) == true)
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH;
                return _rspAddSuona;
            }

            string mename = new BCW.BLL.User().GetUsName(_reqData.userId);
            long megold = new BCW.BLL.User().GetGold(_reqData.userId);

            //每分钟收费多少
            int bMinute = Convert.ToInt32(ub.GetSub("NetworkiGold", xmlPath));

            //检查内容是否超限
            if (Regex.IsMatch(_reqData.content, @"^[^\^]{" + ub.GetSub("NetworksLength", xmlPath) + "," + ub.GetSub("NetworkbLength", xmlPath) + "}$") == false)
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.NETWORK_SUONA_CONTENT_LENGTH_ERROR;
                return _rspAddSuona;
            }

            //检查显示时限
            if (Regex.IsMatch(_reqData.minute.ToString(), @"^[0-9]\d*$") == false)
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.NETWORK_SUONA_TIME_LENGTH_ERROR;
                return _rspAddSuona;
            }

            if (_reqData.minute < 1 || _reqData.minute > Convert.ToInt32(ub.GetSub("NetworkbMinute", xmlPath)))
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.NETWORK_SUONA_TIME_LENGTH_ERROR;
                return _rspAddSuona;
            }


            //扣币
            if (megold < Convert.ToInt64(_reqData.minute * bMinute))
            {
                _rspAddSuona.header.status = ERequestResult.faild;
                _rspAddSuona.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_COBI_NOT_ENOUGH;
                return _rspAddSuona;
            }



            new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, -Convert.ToInt64(_reqData.minute * bMinute), "发布广播");

            BCW.Model.Network model = new BCW.Model.Network();
            model.Title = _reqData.content;
            model.Types = 0;
            model.UsID = _reqData.userId;
            model.UsName = mename;
            model.OverTime = DateTime.Now.AddMinutes(_reqData.minute);
            model.AddTime = DateTime.Now;
            model.OnIDs = "";

            int _id = new BCW.BLL.Network().Add(model);

            _rspAddSuona.header.status = ERequestResult.success;
            _rspAddSuona.suonaId = _id;
            return _rspAddSuona;

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
