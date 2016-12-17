<%@ WebHandler Language="C#" Class="UserAccount" %>

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
using BCW.Mobile.User;


public class UserAccount : IHttpHandler {

    private HttpContext httpContext;

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        httpContext = context;


        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );

        switch( pAct )
        {
            case "get":            //获得所有帐户信息
                GetMedalData();
                break;
            case "modifyPwd":    //修改密码
                ModifyPwd();
                break;
            case "resetPwd":    //忘记密码
                ResetPwd();
                break;
        }


    }

    private void GetMedalData()
    {
        ReqUserAccount _reqData = new ReqUserAccount();
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");

        RspUserAccount _rspData = UserManager.Instance().GetUserAllAccount(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    private void ModifyPwd()
    {
        ReqUserModifyPwd _reqData = new ReqUserModifyPwd();
        _reqData.userId =  int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey =  Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.oldPwd =   Utils.GetRequest("pOldPwd", "post", 0, "", "");
        _reqData.newPwd =   Utils.GetRequest("pNewPwd", "post", 0, "", "");   
        RspUserModifyPwd _rspData = PasswordManager.Instance().UserModifyPwd(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());      
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    private void ResetPwd()
    {
        ReqUserResetPwd _reqData = new ReqUserResetPwd();
        _reqData.accountId =  Utils.GetRequest("pAccount", "post", 1, @"^\d*$", "");
        _reqData.ValidateCode = int.Parse(Utils.GetRequest("pVerifycode", "post", 1, @"^\d*$", "-1"));
        _reqData.newPwd =   Utils.GetRequest("pNewPwd", "post", 0, "", "");   
        RspUserResetPwd _rspData = PasswordManager.Instance().UserResetPwd(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());  
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

