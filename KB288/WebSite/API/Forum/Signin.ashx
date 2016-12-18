<%@ WebHandler Language="C#" Class="Signin" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile;
using BCW.Common;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.BBS.Thread;
using BCW.Mobile.Error;
using BCW.Mobile.Protocol;
using BCW.Mobile.BBS.Signin;

public class Signin : IHttpHandler {

    private HttpContext httpContext;

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        httpContext = context;

        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );

        switch( pAct )
        {
            case "list":            //获取最新签到数据
                GetSigninData();
                break;
            case "sign":
                SetSignin();        //签到
                break;
        }
    }

    private void GetSigninData()
    {
        ReqSignin _reqSigninData = new ReqSignin();
        _reqSigninData.userId =  int.Parse( Utils.GetRequest( "pUserId", "post", 1, @"^\d*$", "-1" ) );
        _reqSigninData.userKey = Utils.GetRequest( "pUsKey", "post", 0, "", "" );

        RspSigninData _rspSigninData = SigninManager.Instance().GetSinginData(_reqSigninData);
        httpContext.Response.Write(_rspSigninData.SerializeObject());
    }

    private void SetSignin()
    {
        ReqSignin _reqSigninData = new ReqSignin();
        _reqSigninData.userId =  int.Parse( Utils.GetRequest( "pUserId", "post", 1, @"^\d*$", "-1" ) );
        _reqSigninData.userKey = Utils.GetRequest( "pUsKey", "post", 0, "", "" );

        RspSignin _rspSigninData = SigninManager.Instance().UserSignin(_reqSigninData);
        httpContext.Response.Write(_rspSigninData.SerializeObject());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

