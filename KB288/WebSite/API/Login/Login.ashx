<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using BCW.Common;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Mobile.Login;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile;


public class Login : IHttpHandler
{
    private HttpContext mContext;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        mContext = context;

        string pAct = context.Request.Form["pAct"];   //操作类型

        if (string.IsNullOrEmpty(pAct) == false)
        {
            switch (pAct)
            {
                case "dsBingPlatform":
                    DesBingingPlatFormId();
                    break;
                case "bingPlatform":
                    BingingPlatFormId();
                    break;
            }
            return;
        }

        string _userId = context.Request.Form["userId"];   //帐号
        string _pwd = context.Request.Form[ "pwd" ];     //密码
        string _bingType = context.Request.Form[ "platformType" ];     //绑定平台类型
        string _assessToken = context.Request.Form[ "platformId" ];     //绑定平台唯一AssessToken

        if( string.IsNullOrEmpty( _userId ) == false && Regex.IsMatch( _userId, @"^\d*" ) == false )
        {
            LoginData _loginData = LoginManager.instance().Error( MOBILE_ERROR_CODE.MOBILE_USERID_VERIFY);
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            context.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
            return;
        }

        LoginBase _loginBase = null;
        try
        {
            if( string.IsNullOrEmpty( _userId ) == false && string.IsNullOrEmpty( _bingType ) == false )
                _loginBase = LoginManager.instance().Login( _userId, _pwd, ( EMobileLoginType ) ( int.Parse( _bingType ) ), _assessToken );
            else if( string.IsNullOrEmpty( _userId ) == false )
                _loginBase = LoginManager.instance().Login(_userId, _pwd );
            else if( string.IsNullOrEmpty( _bingType ) ==false )
                _loginBase = LoginManager.instance().Login( ( EMobileLoginType ) ( int.Parse( _bingType ) ), _assessToken );

            if( _loginBase != null )
            {
                //写入Cookie
                if( _loginBase.rspLoginData.header.status == ERequestResult.success )
                {
                    HttpCookie cookie = new HttpCookie( "LoginComment" );
                    cookie.Expires = DateTime.Now.AddDays( 30 );//30天
                    cookie.Values.Add( "userkeys", DESEncrypt.Encrypt( Utils.Mid( _loginBase.rspLoginData.user.keys, 0, _loginBase.rspLoginData.user.keys.Length - 4 ) ) );
                    HttpContext.Current.Response.Cookies.Add( cookie );

                    //----------------------写入登录日志文件作永久保存
                    try
                    {
                        string MyIP = Utils.GetUsIP();
                        string ipCity = string.Empty;
                        ;
                        if( !string.IsNullOrEmpty( MyIP ) )
                        {

                            ipCity = new IPSearch().GetAddressWithIP( MyIP );
                            if( !string.IsNullOrEmpty( ipCity ) )
                            {
                                ipCity = ipCity.Replace( "IANA保留地址  CZ88.NET", "本机地址" ).Replace( "CZ88.NET", "" ) + ":";
                            }

                            string FilePath = System.Web.HttpContext.Current.Server.MapPath( "/log/loginip/" + _loginBase.rspLoginData.user.userId + "_" + DESEncrypt.Encrypt( _loginBase.rspLoginData.user.userId.ToString(), "kubaoLogenpt" ) + ".log" );
                            LogHelper.Write( FilePath, "" + ipCity + "" + MyIP + "(登录)" );
                        }
                    }
                    catch
                    {
                    }
                }

                JsonSerializerSettings _seting = new JsonSerializerSettings();
                _seting.NullValueHandling = NullValueHandling.Ignore;
                context.Response.Write( JsonConvert.SerializeObject( _loginBase.rspLoginData,Formatting.Indented,_seting ) );
            }

        }
        catch( Exception e )
        {
            LoginData _loginData = LoginManager.instance().Error( MOBILE_ERROR_CODE.LOGIN_PARAMS_ERROR );
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            context.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region 绑定第三方平台
    public void BingingPlatFormId()
    {
        string _userId = mContext.Request.Form["userId"];   //帐号
        string _bingType = mContext.Request.Form[ "platformType" ];     //绑定平台类型
        string _assessToken = mContext.Request.Form[ "platformId" ];     //绑定平台唯一AssessToken
        LoginData _loginData = new LoginData();
        _loginData.user = null;

        //绑定第三方登录
        BCW.Mobile.Model.UserPlatform _userPlatform = new BCW.Mobile.BLL.UserPlatform().GetModel( _assessToken, int.Parse(_bingType ));
        if (_userPlatform == null)
        {
            //检查帐户信息
            BCW.Model.User _user = new BCW.BLL.User().GetBasic(int.Parse(_userId));
            if (_user  == null)
            {
                _loginData.header.status = ERequestResult.faild;
                _loginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND;
                return;
            }

            BCW.Mobile.Model.UserPlatform _newUserPlatform = new BCW.Mobile.Model.UserPlatform();
            _newUserPlatform.platformId = _assessToken;
            _newUserPlatform.platformType = int.Parse(_bingType);
            _newUserPlatform.userId = _user.ID;
            new BCW.Mobile.BLL.UserPlatform().Add(_newUserPlatform);
            _loginData.header.status = ERequestResult.success;
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            mContext.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
        }
        else
        {
            _loginData.header.status = ERequestResult.faild;
            _loginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_BINDPLATFORM_USE;
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            mContext.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
        }

    }
    #endregion

    #region 解除第三方绑定
    public void DesBingingPlatFormId()
    {
        string _bingType = mContext.Request.Form[ "platformType" ];     //绑定平台类型
        string _assessToken = mContext.Request.Form[ "platformId" ];     //绑定平台唯一AssessToken
        LoginData _loginData = new LoginData();
        _loginData.user = null;

        if( string.IsNullOrEmpty( _assessToken ) == true)
        {
            _loginData = LoginManager.instance().Error( MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR);
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            mContext.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
            return;
        }

        //执行解绑
        if (new BCW.Mobile.BLL.UserPlatform().Delete(_assessToken, int.Parse(_bingType)))
        {
            _loginData.header.status = ERequestResult.success;
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            mContext.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
        }
        else
        {
            _loginData.header.status = ERequestResult.faild;
            _loginData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_PLATFORM_USER_NOTFOUND;
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            _seting.NullValueHandling = NullValueHandling.Ignore;
            mContext.Response.Write( JsonConvert.SerializeObject( _loginData,Formatting.Indented,_seting ) );
        }
    }
    #endregion
}