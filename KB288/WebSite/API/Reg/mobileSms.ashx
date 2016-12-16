<%@ WebHandler Language="C#" Class="mobileSms" %>

using System;
using System.Web;
using System.Text.RegularExpressions;
using BCW.Mobile.SMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class mobileSms : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string _mobile = context.Request[ "mobile" ];

        JsonSerializerSettings _setting = new JsonSerializerSettings();
        _setting.NullValueHandling = NullValueHandling.Ignore;
        
        SmsData _smsData = SmsManager.GetInstance().SendSms( _mobile );
        if( _smsData != null )
            context.Response.Write( JsonConvert.SerializeObject( _smsData,Formatting.Indented,_setting ) );
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}