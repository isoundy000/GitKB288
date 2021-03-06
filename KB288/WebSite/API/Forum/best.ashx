﻿<%@ WebHandler Language="C#" Class="best" %>

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

public class best : IHttpHandler {
    private BestInfo bestInfo;
    private HttpContext httpContext;
    private int meid;

    public best()
    {
        bestInfo = new BestInfo();
    }

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        httpContext = context;

        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );

        switch( pAct )
        {
            case "list":            //帖子列表
                ShowThreadList();
                break;
            case "addThread":       //发新贴子
                AddThread();
                break;
            case "editThread":      //编辑贴子
                EditThread();
                break;
            case "delThread":      //删除贴
                DelThread();
                break;
            case "setGood":          //设置精华
                SetGood();
                break;
            case "setTop":          //设置置顶
                SetTop();
                break;
            case "look":            //查看帖子
                LookThread();         
                break;
            default:
                bestInfo.header.status = ERequestResult.faild;
                bestInfo.header.statusCode = MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                context.Response.Write( JsonConvert.SerializeObject( bestInfo ) );
                break;

        }

    }

    /// <summary>
    /// 显示帖子列表
    /// </summary>
    private void ShowThreadList()
    {
        int pThreadId = int.Parse( Utils.GetRequest( "pThreadId", "all", 1, @"^\d*$", "-1" ) );
        int pForumId = int.Parse( Utils.GetRequest( "pForumId", "all", 1, @"^\d*$", "0" ) );
        int pType = int.Parse( Utils.GetRequest( "pType", "all", 1, @"^\d*$", "0" ) );
        int pUserId = int.Parse( Utils.GetRequest( "pUserId", "all", 1, @"^\d*$", "-1" ) );

        bestInfo.header.status = ERequestResult.success;
        bestInfo.InitData( pForumId, pThreadId, pType,pUserId );
        httpContext.Response.Write( JsonConvert.SerializeObject( bestInfo ) );
    }

    /// <summary>
    /// 发贴
    /// </summary>
    private void AddThread()
    {
        ReqAddThread _reqAddThread = new ReqAddThread();
        _reqAddThread.userId =  int.Parse( Utils.GetRequest( "pUserId", "post", 1, @"^\d*$", "-1" ) );
        _reqAddThread.userKey = Utils.GetRequest( "pUsKey", "post", 0, "", "" );
        _reqAddThread.forumId = int.Parse( Utils.GetRequest( "pForumId", "post", 1, @"^\d*$", "-1" ) );
        _reqAddThread.pType = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[0-4]$|^6$|^7$|^8$", "-1"));
        _reqAddThread.title= Utils.GetRequest("pTitle", "post", 0, "","");
        _reqAddThread.content = Utils.GetRequest("pContent", "post",  0, "","");

        RspAddThread _rspData =  bestInfo.AddThread(_reqAddThread);
        httpContext.Response.Write( _rspData.SerializeObject());
    }

    private void EditThread()
    {
        ReqEditThread _reqEditThread = new ReqEditThread();
        _reqEditThread.userId = int.Parse( Utils.GetRequest( "pUserId", "post", 1, @"^\d*$", "-1" ) );
        _reqEditThread.userKey = Utils.GetRequest( "pUsKey", "post", 0, "", "" );
        _reqEditThread.threadId = int.Parse( Utils.GetRequest( "pThreadId", "post", 1, @"^\d*$", "-1" ) );
        _reqEditThread.title= Utils.GetRequest("pTitle", "post", 0, "","");
        _reqEditThread.content = Utils.GetRequest("pContent", "post",  0, "","");

        RspEditThread _rspData =  bestInfo.EditThread(_reqEditThread);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void DelThread()
    {
        ReqDelThread _reqDelThread = new ReqDelThread();
        _reqDelThread.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqDelThread.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqDelThread.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));

        RspDelThread _rspData = bestInfo.DelThread(_reqDelThread);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void SetTop()
    {
        ReqTopThread _reqTopThread = new ReqTopThread();
        _reqTopThread.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqTopThread.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqTopThread.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));
        _reqTopThread.topType = int.Parse(Utils.GetRequest("pTopType", "post", 1, @"^\d*$", "-1"));

        RspTopThread _rspData = bestInfo.SetTopThread(_reqTopThread);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void SetGood()
    {
        ReqGoodThread _reqGoodThread = new ReqGoodThread();
        _reqGoodThread.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqGoodThread.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqGoodThread.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));
        _reqGoodThread.goodType = int.Parse(Utils.GetRequest("pGoodType", "post", 1, @"^\d*$", "-1"));


        RspGoodThread _rspData = bestInfo.SetGoodThread(_reqGoodThread);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void LookThread()
    {
        ReqLookThread _reqData = new ReqLookThread();
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));

        RspLookThread _rspData = bestInfo.LookThread(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}

