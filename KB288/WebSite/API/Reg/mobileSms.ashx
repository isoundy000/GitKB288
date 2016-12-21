<%@ WebHandler Language="C#" Class="mobileSms" %>

using System;
using System.Web;
using System.Text.RegularExpressions;
using BCW.Mobile.SMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Common;
using BCW.Mobile.Error;
using BCW.Mobile.Home;

public class mobileSms : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string _mobile = context.Request[ "mobile" ];
        int _type = int.Parse( Utils.GetRequest( "pType", "all", 1, @"^\d*$", "1" ) );

        JsonSerializerSettings _setting = new JsonSerializerSettings();
        _setting.NullValueHandling = NullValueHandling.Ignore;

        //如果是ID，尝试查找ID对应的手机号码
        if (_mobile.Length < 11)
        {
            //检查用户ID是否合法
            if (Regex.IsMatch(_mobile, @"^[0-9]\d*$") == false)
            {
                Header _header = new Header();
                _header.status = BCW.Mobile.ERequestResult.faild;
                _header.statusCode = MOBILE_ERROR_CODE.SYS_USER_ACCOUNT_VERIFY;
                context.Response.Write(JsonConvert.SerializeObject(_header));
                return;
            }

            //帐号是否存在
            if (new BCW.BLL.User().Exists(int.Parse(_mobile)) == false)
            {
                Header _header = new Header();
                _header.status = BCW.Mobile.ERequestResult.faild;
                _header.statusCode = MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND;
                context.Response.Write(JsonConvert.SerializeObject(_header));
                return;
            }

            _mobile = new BCW.BLL.User().GetMobile(int.Parse(_mobile));
        }

        SmsData _smsData = SmsManager.GetInstance().SendSms( _mobile ,_type);
        if( _smsData != null )
            context.Response.Write( JsonConvert.SerializeObject( _smsData,Formatting.Indented,_setting ) );
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}