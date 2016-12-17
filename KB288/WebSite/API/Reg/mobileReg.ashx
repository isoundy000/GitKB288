<%@ WebHandler Language="C#" Class="MobileRegHandler" %>

using System;
using System.Web;
using System.Text.RegularExpressions;
using BCW.Mobile;
using BCW.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class MobileRegHandler : IHttpHandler {
    private MobileReg objMobileReg;


    public MobileRegHandler()
    {
        objMobileReg = new MobileReg();
    }
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string mobile = context.Request.Form[ "mobile" ];
        string verifyCode = context.Request.Form[ "verifycode" ];
        string pwd = context.Request.Form[ "pwd" ];
        string pwdr = context.Request.Form[ "pwdr" ];

        JsonSerializerSettings _setting = new JsonSerializerSettings();
        _setting.NullValueHandling = NullValueHandling.Ignore;
        
        objMobileReg.Register( mobile, verifyCode, pwd, pwdr );
        context.Response.Write( JsonConvert.SerializeObject( objMobileReg,Formatting.Indented,_setting) );
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}