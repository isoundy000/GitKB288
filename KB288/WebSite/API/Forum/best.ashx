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
    private int meid;

    public best()
    {
        bestInfo = new BestInfo();
    }    
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );

        if( pAct == "list" )
        {
            int pThreadId = int.Parse( Utils.GetRequest( "pThreadId", "all", 1, @"^\d*$", "-1" ) );
            int pForumId = int.Parse( Utils.GetRequest( "pForumId", "all", 1, @"^\d*$", "0" ) );
            int pType = int.Parse( Utils.GetRequest( "pType", "all", 1, @"^\d*$", "0" ) );
            
            bestInfo.header.status = ERequestResult.success;
            bestInfo.header.statusMsg = "";
            
            bestInfo.InitData( pForumId, pThreadId, pType );
            context.Response.Write(JsonConvert.SerializeObject(bestInfo));
        }
        else
        {
            bestInfo.header.status = ERequestResult.faild;
            bestInfo.header.statusMsg = "页面请求参数错误";
            context.Response.Write(JsonConvert.SerializeObject(bestInfo)); 
        }
            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

