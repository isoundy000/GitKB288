using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Home;
using BCW.Mobile;
using BCW.Common;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Error;


namespace BCW.Mobile.Login
{
    public enum EMobileLoginType
    {
       account,             //账号密码登录 
       weChat,              //微信登录 
       qq,                  //qq
       sinaBlog,                //新浪微博
    }

    public class LoginUserInfo
    {
        public int userId;          //用户ID
        public string userName;     //用户昵称
        public string userImg;      //用户头像
        public string userKey;      //用户U值(尾巴)
        public string platformId;   //第三方平台ID

        [JsonIgnore]
        public string keys;
        
    }

    public class LoginData
    {
        public Header header;       //头信息
        public LoginUserInfo user;

        public LoginData()
        {
            header = new Header();
            user = new LoginUserInfo();
        }
    }

    #region 基类
    public abstract class LoginBase
    {
        public LoginData rspLoginData;  

        protected EMobileLoginType loginType;

        public LoginBase()
        {
            this.Init();
            rspLoginData = new LoginData();
        }

        public abstract void Init();

        public void Login(string _account,string _pwd,bool platform = false)
        {
           //检查用户密码是否正确
            int _userRow = 0;
            string _md5Pwd = platform == true ? _pwd : Utils.MD5Str(_pwd);
            BCW.Model.User _user = new BCW.Model.User();             
            _user.UsPwd = _md5Pwd;
            if( _account.ToString().Length == 11 )
            {
                _user.Mobile = _account;
                _userRow = new BCW.BLL.User().GetRowByMobile( _user );
            }
            else
            {
                _user.ID = int.Parse(_account);
                _userRow = new BCW.BLL.User().GetRowByID( _user );
            }

            if( _userRow <=0)
            {
                rspLoginData.header.status = ERequestResult.faild;
                rspLoginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_USER_PWD_ERROR;
                return;
            }

            _user = new BCW.BLL.User().GetKey( _userRow );


            int UsId = _user.ID;
            string UsKey = _user.UsKey;
            string UsPwd = _user.UsPwd;

            BCW.Model.User modelgetbasic = new BCW.BLL.User().GetBasic( _user.ID );

            //设置keys
            string keys = "";
            keys = BCW.User.Users.SetUserKeys( UsId, UsPwd, UsKey );
            string bUrl = string.Empty;
            if( Utils.getPage( 1 ) != "" )
            {
                bUrl = Utils.getUrl( Utils.removeUVe( Utils.getPage( 1 ) ) );
            }
            else
            {
                bUrl = Utils.getUrl( "/default.aspx" );
            }
            //更新识别串
            string SID = ConfigHelper.GetConfigString( "SID" );
            bUrl = UrlOper.UpdateParam( bUrl, SID, keys );


            //----------------------写入日志文件作永久保存
            new BCW.BLL.User().UpdateTime( UsId );
            //APP全部在线登录
            new BCW.BLL.User().UpdateState( UsId, 0 );

            TimeSpan timediff = DateTime.Now - Convert.ToDateTime( "1970-01-01 00:00:00" );
            long stt = ( Int64 ) timediff.TotalMilliseconds;

            rspLoginData.header.status = ERequestResult.success;
            rspLoginData.user.keys = keys;
            rspLoginData.user.userId = UsId;
            rspLoginData.user.userName = modelgetbasic.UsName;
            rspLoginData.user.userImg = "http://"+ Utils.GetDomain()+modelgetbasic.Photo;
            rspLoginData.user.userKey = UsKey; 
        }
    }
    #endregion           



    #region 第三方平台帐号登录基类
    public class Loginplatform : LoginBase
    {
        protected string assessToken;                       //第三方访问用户标识(唯一的)

        public Loginplatform( string _assessToken )
            : base()
        {
            this.assessToken = _assessToken;
        }

        public void Login(string _assessToken)
        {
            //是否已绑定关联帐号
            Model.UserPlatform _userPlatform = new BLL.UserPlatform().GetModel( _assessToken, ( int ) this.loginType );
            if( _userPlatform == null )
            {
                rspLoginData.header.status = ERequestResult.faild;
                rspLoginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_PLATFORM_USER_NOTFOUND;             
                return;
            }

            //关联帐号是否有效
            if( new BCW.BLL.User().Exists( _userPlatform.userId ) == false)
            {
                rspLoginData.header.status = ERequestResult.faild;
                rspLoginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND;
                return;
            }

            rspLoginData.user.platformId = _userPlatform.platformId;
            base.Login(_userPlatform.userId.ToString(),new BCW.BLL.User().GetUsPwd(_userPlatform.userId),true);               //执行普通帐号密码登录
        }

        public override void Init()
        {
            ;
        }

    }
    #endregion


    #region 帐号密码登录
    public class LoginAccount : LoginBase
    {
        /// <summary>
        /// 初始化登录类型为：普通帐号密码登录 
        /// </summary>
        public override void Init()
        {
            this.loginType = EMobileLoginType.account;          //指定类型
        }

        /// <summary>
        /// 登录同时绑定第三方登录平台信息
        /// </summary>
        /// <param name="_account">登录用户名</param>
        /// <param name="_pwd">登录密码</param>
        /// <param name="_type">绑定的第三方平台类型(1:微信  2：QQ  3:新浪微博)</param>
        /// <param name="_assessToken"></param>
        public void Login( string _account, string _pwd, EMobileLoginType _type, string _assessToken )
        {   
            //绑定第三方登录
            Model.UserPlatform _userPlatform = new BLL.UserPlatform().GetModel( _assessToken, ( int ) this.loginType );
            if( _userPlatform == null )
            {
                //检查用户密码是否正确
                int _userRow = 0;
                string _md5Pwd = Utils.MD5Str( _pwd );
                BCW.Model.User _user = new BCW.Model.User();
                _user.UsPwd = _md5Pwd;
                if( _account.ToString().Length == 11 )
                {
                    _user.Mobile = _account;
                    _userRow = new BCW.BLL.User().GetRowByMobile( _user );
                }
                else
                {
                    _user.ID = int.Parse( _account );
                    _userRow = new BCW.BLL.User().GetRowByID( _user );
                }

                if( _userRow <= 0 )
                {
                    rspLoginData.header.status = ERequestResult.faild;
                    rspLoginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_USER_PWD_ERROR;
                    return;
                }

                _user = new BCW.BLL.User().GetKey( _userRow ); 

                Model.UserPlatform _newUserPlatform = new BCW.Mobile.Model.UserPlatform();
                try
                {
                    _newUserPlatform.platformId = _assessToken;
                    _newUserPlatform.platformType = ( int ) _type;
                    _newUserPlatform.userId = _user.ID;
                    rspLoginData.user.platformId = _assessToken;
                    new BLL.UserPlatform().Add( _newUserPlatform );
                }
                catch( Exception e )
                {
                    ;
                }
            } 


            base.Login( _account, _pwd );



        }
    }
    #endregion


    #region 微信登录
    public class LoginWeChat : Loginplatform
    {
        public LoginWeChat( string _assessToken )
            : base( _assessToken )
        {

        }

        /// <summary>
        /// 初始化登录类型为：微信账号登录 
        /// </summary>
        public override void Init()
        {
            this.loginType = EMobileLoginType.weChat;          //指定类型为微信
        }
      
    }
    #endregion

    #region QQ登录
    public class LoginQQ : Loginplatform
    {
        public LoginQQ( string _assessToken )
            : base( _assessToken )
        {

        }

        /// <summary>
        /// 初始化登录类型为：QQ账号登录 
        /// </summary>
        public override void Init()
        {
            this.loginType = EMobileLoginType.qq;             //指定类型为QQ
        }
    }
    #endregion

    #region 新浪微博登录
    public class LoginSinaBlog : Loginplatform
    {
        public LoginSinaBlog( string _assessToken )
            : base( _assessToken )
        {

        }

        /// <summary>
        /// 初始化登录类型为：新浪微博账号登录 
        /// </summary>
        public override void Init()
        {
            this.loginType = EMobileLoginType.sinaBlog;      //指定类型为新浪微博
        }
    }
    #endregion

}
