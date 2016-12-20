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

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string _userId = context.Request.Form["userId"];   //帐号
        string _pwd = context.Request.Form[ "pwd" ];     //密码
        string _bingType = context.Request.Form[ "platformType" ];     //绑定平台类型
        string _assessToken = context.Request.Form[ "platformId" ];     //绑定平台唯一AssessToken

        if( Regex.IsMatch( _userId, @"^\d*" ) == false )
        {
            LoginData _loginData = LoginManager.instance().Error( MOBILE_ERROR_CODE.MOBILE_USERID_VERIFY);
            context.Response.Write( JsonConvert.SerializeObject( _loginData ) );  
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
            context.Response.Write( JsonConvert.SerializeObject( _loginData ) );
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region 登录判断
    public static string LoginStatue(int i, string a, string b, string c, string d, string e, string f, string g)
    {
        //{"status":"1", msg:"success"}
        if (i == 1)//登录成功
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{" + "");
            jsonBuilder.Append("\"" + "status" + "\"" + ": " + "\"" + "SUCCESS" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "status_msg" + "\"" + ":" + "\"" + "" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "accesstoken" + "\"" + ":" + "\"" + a + "\"");//身份识别字符串u
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "servertime" + "\"" + ":" + "\"" + b + "\"");//当前服务器时间
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "accesstoken_expire" + "\"" + ":" + "\"" + c + "\"");//accesstoken到期时间
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "userInfo" + "\"" + ":");
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "uid" + "\"" + ":" + "\"" + d + "\"");//用户id
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "face" + "\"" + ":" + "\"" + e + "\"");//头像url
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "nickname" + "\"" + ":" + "\"" + f + "\"");//昵称
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "level" + "\"" + ":" + "\"" + g + "\"");//等级数字
            jsonBuilder.Append("}");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        else if (i == 2)//登录失败
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "status" + "\"" + ":" + "\"" + "FAIL" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "status_msg" + "\"" + ":" + "\"" + "账号或密码错误" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "err_code" + "\"" + ":" + "\"" + "701" + "\"");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        else
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "status" + "\"" + ":" + "\"" + 3 + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "msg" + "\"" + ":" + "\"" + "error" + "\"");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }
    #endregion
}