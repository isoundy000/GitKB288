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

    public enum ERequestResult                
    {
        success,
        faild,
    }

    public enum EPageType
    {
        eHome,
        eBbs
    }



    /// <summary>
    /// 错误码定义类
    /// </summary>
    public static class MOBILE_ERROR_CODE
    {
        /// <summary>
        ///  系统出现未知错误，请稍候再试...
        /// </summary>
        public const string MOBILE_SYS_ERROR = "系统出现未知错误，请稍候再试...";

        /// <summary>
        /// 服务器繁忙，请稍候再试...
        /// </summary>
        public const string MOBILE_SYS_BUSY = "服务器繁忙，请稍候再试...";

        /// <summary>
        /// 注册手机号码为空值 
        /// </summary>
        public const string MOBILE_PHONE_ISNULL = "手机号码不能为空";

        /// <summary>
        /// 手机号码格式不正确
        /// </summary>
        public const string MOBILE_PHONE_VERIFY = "请输入正确格式的11位数手机号码";  


        #region 短信验证码

                /// <summary>
        /// 今日获取短信太频繁，已达本日上限，请明天再试
        /// </summary>
        public const string SMS_FREQUENTLY_TODAY = "今日获取短信太频繁，已达本日上限，请明天再试";

       /// <summary>
        ///  之前存在频繁获取短信，请明天再试
       /// </summary>
        public const string SMS_FREQUENTLY_FLAG = "之前存在频繁获取短信，请明天再试";

        /// <summary>
        ///  同一IP由于频繁获取短信，请明天再试
        /// </summary>
        public const string SMS_FREQUENTLY_IP = "该IP获取短信太频繁，请明天再试";

        /// <summary>
        /// 该号码由于频繁获取短信，请明天再试
        /// </summary>
        public const string SMS_FREQUENTLY_PHONE = "该号码获取短信太频繁，请明天再试"; 

        #endregion


        #region  会员注册
        
        /// <summary>
        ///  手机号码已经注册
        /// </summary>
        public const string REGEDIT_MOBILE_EXISTS = "该手机号码已经注册";

       /// <summary>
        /// 密码限6-20位,必须由字母或数字组成
       /// </summary>
        public const string REGEDIT_PWD_VERIFY = "密码限6-20位,必须由字母或数字组成";

        /// <summary>
        /// 确认密码限6-20位,必须由字母或数字组成
        /// </summary>
        public const string REGEDIT_PWDR_VERIFY = "确认密码限6-20位,必须由字母或数字组成";

        /// <summary>
        /// 两次密码输入不一致
        /// </summary>
        public const string REGEDIT_PWD_DIFF = "两次密码输入不一致"; 

        
        /// <summary>
        /// 验证码已过期
        /// </summary>
        public const string REGEDIT_VERIFYCODE_EXPIRE = "验证码已过期，请重新获取";

        /// <summary>
        /// 请输入正确的验证码
        /// </summary>
        public const string REGEDIT_VERIFYCODE_DIFF = "请输入正确的验证码"; 
            
        #endregion
    }

    /// <summary>
    /// 轮播内容. 
    /// </summary>
    public class sliderPicItem
    {
        public string url;
        public string content_type;
        public string[] arryParams; 
    }


    /// <summary>
    /// 轮播对象
    /// </summary>
    public class sliderPic
    {
        private int count;

        [JsonProperty]
        private List<sliderPicItem> items;

        public sliderPic()
        {
            items = new List<sliderPicItem>();
        }

        public void InitData(EPageType type)
        {
            DataSet _ds = new BCW.MobileSlider.BLL.MobileSlider().GetList( "ptype=" + ( int ) type );

            for( int i = 0; i < _ds.Tables[0].Rows.Count; i++ )
            {
                sliderPicItem _item = new sliderPicItem();
                _item.url ="http://"+Utils.GetDomain()+_ds.Tables[ 0 ].Rows[ i ][ "url" ].ToString();
                _item.content_type = _ds.Tables[ 0 ].Rows[ i ][ "contentType" ].ToString();
                _item.arryParams = _ds.Tables[ 0 ].Rows[ i ][ "param" ].ToString().Split( '|' );
                items.Add( _item );
            }
        }
    }

     


    /// <summary>
    /// 贴子内容
    /// </summary>
    public class EssencePostItem
    {
        public int threadId;                 //贴子ID
        public int authorId;                //作者id
        public string author;                //作者
        public string authorImg;             //作者头像   
        public int forumId;                  //论坛栏目ID
        public string title;                 //帖子标题
        public string content;               //贴子内容
        public string preview;               //预览图
        public string forum;                 //论坛栏目名称
        public int views;                    //阅读数
        public int replys;                   //评论数
        public int likes;                    //喜欢数 
        public long addTime;            //发贴时间
        public int IsGood;                  //是否精华
        public int IsRecom;                 //是否推荐
        public int IsLock;                  //是否锁定
        public int IsTop;                   //是否置顶
    }

    /// <summary>
    /// 论坛精华贴
    /// </summary>
    public class EssencePost
    {
        public List<EssencePostItem> items;
        public bool finish;
        public const int RECORD_COUNT = 10;

        public EssencePost()
        {
            items = new List<EssencePostItem>();             
        }
                 
        public void InitData( int ForumId ,int postId, int ptype )
        {

            this.finish = true;
            items.Clear();              

            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "act", "forumid", "tsid", "ptype", "backurl" };

            strWhere = "IsDel=0 and HideType<>9";

            if( ptype == 1 )
                strWhere += " and IsGood=1";
            else if( ptype == 2 )
                strWhere += " and IsRecom=1";
            if( ptype == 3 )
                strWhere += " and AddTime>='" + DateTime.Now.AddDays( -2 ) + "'";
            else if( ptype == 4 )
                strWhere += " and IsLock=1";
            else if( ptype == 5 )
                strWhere += " and IsTop=-1";

            if( ForumId > 0)
                strWhere += " and ForumId=" + ForumId.ToString();

            if( postId >= 0 )
             strWhere += " and 1=1";

            //排序条件
            if( ForumId > 0 && postId <0 )
                strOrder += "IsTop desc,";
            strOrder += "ID Desc";

            DataSet _ds = new BCW.BLL.Text().GetList(string.Format( "TOP {0} ID,ForumId,Types,LabelId,Title,Content,UsID,UsName,ReplyNum,ReadNum,IsGood,IsRecom,IsLock,IsTop,IsOver,AddTime,Gaddnum,Gwinnum,GoodSmallIcon", RECORD_COUNT ), strWhere.Replace("1=1","ID<"+postId) +" Order by "+strOrder );
            if( _ds.Tables[ 0 ].Rows.Count > 0 )
            {     
                for (int i=0;i<_ds.Tables[ 0 ].Rows.Count;i++)
                {
                    EssencePostItem _essencePostItem = new EssencePostItem();
                    _essencePostItem.threadId = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ID" ].ToString() );
                    _essencePostItem.authorId = int.Parse(_ds.Tables[ 0 ].Rows[ i ][ "UsID" ].ToString());
                    _essencePostItem.author = _ds.Tables[ 0 ].Rows[ i ][ "UsName" ].ToString();
                    _essencePostItem.authorImg = "http://" + Utils.GetDomain() + new BCW.BLL.User().GetPhoto( int.Parse(_ds.Tables[ 0 ].Rows[ i ][ "UsID" ].ToString()) );
                    _essencePostItem.forumId = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ForumId" ].ToString() );
                    _essencePostItem.title = _ds.Tables[ 0 ].Rows[ i ][ "Title" ].ToString().Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
                    _essencePostItem.content = Out.SysUBB( _ds.Tables[ 0 ].Rows[ i ][ "Content" ].ToString() ).Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" ).Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
                    _essencePostItem.preview = string.IsNullOrEmpty( _ds.Tables[ 0 ].Rows[ i ][ "GoodSmallIcon" ].ToString() ) ? "http://" + Utils.GetDomain() + "/Files/threadImg/def.png" : _ds.Tables[ 0 ].Rows[ i ][ "GoodSmallIcon" ].ToString();
                    BCW.Model.Forum _forummodel = new BCW.BLL.Forum().GetForum( _essencePostItem.forumId );
                    _essencePostItem.forum = _forummodel != null ? _forummodel.Title : "";
                    _essencePostItem.views = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ReadNum" ].ToString() );
                    _essencePostItem.replys = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "ReplyNum" ].ToString() );
                    System.DateTime _startTime = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime( 1970, 1, 1 ) );          
                    _essencePostItem.addTime = (long)(DateTime.Parse(_ds.Tables[ 0 ].Rows[ i ][ "AddTime" ].ToString()) - _startTime).TotalSeconds;

                    //打赏
                    DataSet _dsCent = new BCW.BLL.Textcent().GetList( "isnull(SUM(Cents),0)cents", "BID='" + _essencePostItem.threadId + "'" );
                    _essencePostItem.likes = int.Parse( _dsCent.Tables[ 0 ].Rows[ 0 ][ "cents" ].ToString() );

                    _essencePostItem.IsGood = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "IsGood" ].ToString() );
                    _essencePostItem.IsRecom = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "IsRecom" ].ToString() );
                    _essencePostItem.IsLock = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "IsLock" ].ToString() );
                    _essencePostItem.IsTop = int.Parse( _ds.Tables[ 0 ].Rows[ i ][ "IsTop" ].ToString() );

                    items.Add( _essencePostItem );

                    //检查是否到底
                    if( i == _ds.Tables[ 0 ].Rows.Count - 1 )
                    {
                        if (strWhere.Contains("1=1")== false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Text().GetList( "TOP 1 * ", strWhere.Replace( "1=1", "ID<" + _essencePostItem.threadId ) + " Order by " + strOrder );
                        finish = _dsCheck.Tables[ 0 ].Rows.Count <= 0;
                    }
                }
            }  
            
        }   
    }
      

    public class BestInfo
    {
        public Header header;
        public EssencePost bests;    //论坛精华贴         

        public BestInfo()
        {
            bests = new EssencePost();
            header = new Header();
        }


        public void InitData( int ForumId, int _pIndex, int pType )
        {
            bests.InitData( ForumId, _pIndex, pType );
        }


    }


}
