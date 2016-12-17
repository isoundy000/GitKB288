using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BCW.Mobile.Home;
using BCW.Common;

namespace BCW.Mobile.BBS
{
    /// <summary>
    /// 论坛板块
    /// </summary>
    public class Forum : HomeBaseInfo
    {
        public Dictionary<string,List<ForumItem>> dctForumItem;        //论坛版块列表
        private string xmlPath = "/Controls/AppConfig.xml";

        public Forum()
        {
            dctForumItem = new Dictionary<string, List<ForumItem>>();
        }

        //初始化数据
        public void InitData()
        {
            string[] _arrForumAreaName = ub.GetSub( "forumAreaName", xmlPath ).Split( '|' );
            string[] _arrForumAreaId = ub.GetSub( "forumAreaId", xmlPath ).Split( '|' );

            for( int _index = 0; _index < _arrForumAreaName.Length; _index++ )
            {
                List<ForumItem> _lstForumItem = new List<ForumItem>();  
                DataSet _ds = new BCW.BLL.Forum().GetList( "ID,Title,Logo", string.Format( "ID in ({0})", _arrForumAreaId[ _index ] ) );
                if( _ds.Tables[ 0 ].Rows.Count > 0 )
                {
                    for( int i = 0; i < _ds.Tables[ 0 ].Rows.Count; i++ )
                    {
                        ForumItem _forumItem = new ForumItem();
                        _forumItem.forumId = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ID" ].ToString() );
                        _forumItem.forumName = _ds.Tables[ 0 ].Rows[ i ][ "Title" ].ToString();
                        _forumItem.forumLogo = _ds.Tables[ 0 ].Rows[ i ][ "Logo" ].ToString();

                        DataSet _ds2 = new BCW.BLL.Forumstat().GetList( "ISNULL(SUM(tTotal),0)tTotal,ISNULL((SUM(tTotal)+ sum(rTotal)),0)rTotal", "ForumID=" + _forumItem.forumId );
                        if( _ds2.Tables[ 0 ].Rows.Count > 0 )
                        {
                            _forumItem.themeAmount = int.Parse( _ds2.Tables[ 0 ].Rows[ 0 ][ "tTotal" ].ToString() );
                            _forumItem.postAmount = int.Parse( _ds2.Tables[ 0 ].Rows[ 0 ][ "rTotal" ].ToString() );
                        }

                        DataSet _ds3 = new BCW.BLL.Forumstat().GetList( "COUNT(*)todayPost", "ForumId=" + _forumItem.forumId + " and CONVERT(VARCHAR(10),AddTime,120)= CONVERT(VARCHAR(10),GETDATE(),120)" );
                        if( _ds3.Tables[ 0 ].Rows.Count > 0 )
                        {
                            _forumItem.todayPostAmount = int.Parse( _ds3.Tables[ 0 ].Rows[ 0 ][ "todayPost" ].ToString() );
                        }

                        _lstForumItem.Add( _forumItem );

                    }

                }

                if( _lstForumItem.Count > 0 )
                {
                    dctForumItem.Add( _arrForumAreaName[ _index ], _lstForumItem );
                }  
            }

 
        }

        private string GetLstForumItemStr()
        {
            string _str = "";
            foreach( KeyValuePair<string, List<ForumItem>> _keyVal in dctForumItem )
            {
                _str += "{\"areaName\":\"" + _keyVal.Key +"\",";
                _str += "\"items\":[";
                foreach( ForumItem _item in _keyVal.Value )
                {
                    _str += _item.OutPutJsonStr() + ",";
                }

                if( _str != "" )
                    _str = _str.Substring( 0, _str.Length - 1 );

                _str += "]},";

            }

            if( _str != "" )
                _str = _str.Substring( 0, _str.Length - 1 );

            return _str;
        }

        public override string OutPutJsonStr()
        {
            jsonBuilder.Append( "\"forums\":[" + this.GetLstForumItemStr()+"]");
            return jsonBuilder.ToString();
        }
    }


    public class ForumItem : HomeBaseInfo
    {
        public int forumId;             //板块ID
        public string forumName;        //板块名称
        public int themeAmount;         //主题数量            
        public int postAmount;          //贴子数量
        public string forumLogo;        //论坛Logo
        public int todayPostAmount;     //今日贴子数

        public override string OutPutJsonStr()
        {
            jsonBuilder.Append( "{" );
            jsonBuilder.Append( string.Format( "\"forumId\":{0},", this.forumId ) );
            jsonBuilder.Append( string.Format( "\"forumName\":\"{0}\",", this.forumName ) );
            jsonBuilder.Append( string.Format( "\"themeAmount\":{0},", this.themeAmount ) );
            jsonBuilder.Append( string.Format( "\"postAmount\":{0},", this.postAmount ) );
            jsonBuilder.Append( string.Format( "\"forumLogo\":\"{0}\",", this.forumLogo ) );
            jsonBuilder.Append( string.Format( "\"todayPostAmount\":{0}", this.todayPostAmount ) );
            jsonBuilder.Append( "}" );

            return jsonBuilder.ToString();
        }
    }


    public class ForumInfo : HomeBaseInfo
    {
        public Forum forum;  
        public Header header;
        public sliderPic sliderPic;
        public Notices notices;

        public ForumInfo()
        {
            header = new Header();
            forum = new Forum();
            forum.InitData();
        }

        //初始化轮播.
        public void InitSlider()
        {
            sliderPic = new sliderPic();
            sliderPic.InitData( EPageType.eBbs );
        }

        //初始化喇叭及公告
        public void InitNotice()
        {
            notices = new Notices( ENoticeType.e_all );
            ( ( NoticesAllItem ) notices.noticesItem ).InitData( 0 );
        }

        public override string OutPutJsonStr()
        {
            jsonBuilder.Append( "{" );
            jsonBuilder.Append(header.OutPutJsonStr());      

            if( sliderPic != null )
                jsonBuilder.Append( "," + sliderPic.OutPutJsonStr() );

            if( notices != null )
                jsonBuilder.Append( "," + notices.OutPutJsonStr() );

            jsonBuilder.Append( ","+this.forum.OutPutJsonStr() );   

            jsonBuilder.Append( "}" );

            return jsonBuilder.ToString();
        }
    }
}
