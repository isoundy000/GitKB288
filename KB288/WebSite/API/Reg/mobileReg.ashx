<%@ WebHandler Language="C#" Class="MobileRegHandler" %>

using System;
using System.Web;
using System.Text.RegularExpressions;
using BCW.Mobile;
using BCW.Common;


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

        context.Response.Write( Utils.GetUsIP() );
           
        
        
        //objMobileReg.Register( mobile, verifyCode, pwd, pwdr ); 
            
       // if (Regex.IsMatch(mobile,@"^(?:11|12|13|14|15|16|17|18|19)\d{9}$") == false)
           
             
        
        
        
        //Utils.GetRequest( "mobile", "post", 2, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$", "请正确输入十一位数的手机号码" );
        //string code = Utils.GetRequest( "code", "post", 2, @"^[0-9]{4}$", "请输入验证码!" );  //界面图形验证码   
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}