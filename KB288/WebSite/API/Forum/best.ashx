<%@ WebHandler Language="C#" Class="best" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile;
using BCW.Common;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            case "addThread":       //发新贴
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

        bestInfo.header.status = ERequestResult.success;

        bestInfo.InitData( pForumId, pThreadId, pType );
        httpContext.Response.Write( JsonConvert.SerializeObject( bestInfo ) ); 
    }

    /// <summary>
    /// 发贴
    /// </summary>
    private void AddThread()
    {
        int pForumId = int.Parse( Utils.GetRequest( "pForumId", "all", 1, @"^\d*$", "0" ) );
  
        
        //bestInfo.header.status = ERequestResult.success;
        //httpContext.Response.Write( JsonConvert.SerializeObject( bestInfo ) );
    }
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

