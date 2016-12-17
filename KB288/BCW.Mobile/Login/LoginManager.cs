using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Error;


namespace BCW.Mobile.Login
{
    public class LoginManager
    {
       private static LoginManager mInstance;
       private LoginBase loginData;

       public static LoginManager instance()
       {
           if( mInstance == null )
               mInstance = new LoginManager();
           return mInstance;
       }
         
       //帐号密码.
       public LoginBase Login( string _account, string _pwd )
       {
           loginData = null;
           loginData = new LoginAccount();
           loginData.Login( _account, _pwd );
           return loginData;
       }

       /// <summary>
       /// 帐号密码绑定第三方平台并登录
       /// </summary>
       /// <param name="_account">帐号</param>
       /// <param name="_pwd">密码</param>
       /// <param name="_type">绑定平台类型</param>
       /// <param name="_assessToken">用户AssessToken</param>
       /// <returns></returns>  
       public LoginBase Login( string _account, string _pwd, EMobileLoginType _type, string _assessToken )
       {
           loginData = null;
           loginData = new LoginAccount();
           (( LoginAccount ) loginData ).Login( _account, _pwd, _type, _assessToken );
           return loginData;
       }

       //第三方登录
       public LoginBase Login( EMobileLoginType _type, string _assessToken )
       {
           loginData = null;
           switch( _type )
           {
               case EMobileLoginType.weChat:
                   loginData = new LoginWeChat( _assessToken );
                   break;
               case EMobileLoginType.qq:
                   loginData = new LoginQQ( _assessToken );
                   break;
               case EMobileLoginType.sinaBlog:
                   loginData = new LoginSinaBlog( _assessToken );
                   break;
           }

           if( loginData != null )
               ((Loginplatform)loginData).Login( _assessToken );

           return loginData;
       }

       public LoginData Error( MOBILE_ERROR_CODE _errorCode)
       {
           LoginData _loginData = new LoginData();
           _loginData.header.status = ERequestResult.faild;
           _loginData.header.statusCode = _errorCode;
           return _loginData;
       }

    }
}
