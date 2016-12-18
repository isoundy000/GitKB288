<%@ WebHandler Language="C#" Class="text" %>

using System;
using System.Web;
using BCW.Mobile.Home;
using BCW.Common;
using BCW.Mobile.BBS;
using BCW.Mobile;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class text : IHttpHandler {
    private ForumInfo forumInfo;

    public text()
    {
        forumInfo = new ForumInfo();
    }   
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        string _act = Utils.GetRequest( "pAct", "all", 1, "", "" );

        if( _act == "list" )
        {
            //论坛精华 

            forumInfo.header.status = ERequestResult.success;
            forumInfo.header.statusMsg = "";

            //是否请求轮播
            string _reqSlider = Utils.GetRequest( "pReqSlider", "all", 1, "", "" );
            if( _reqSlider == "true" )
                forumInfo.InitSlider();

            //是否请求喇叭公告
            string _reqNotice = Utils.GetRequest( "pReqNotice", "all", 1, "", "" );
            if( _reqNotice == "true" )
                forumInfo.InitNotice();

            context.Response.Write( JsonConvert.SerializeObject(forumInfo) );

        }
        else
        {
            forumInfo.header.status = ERequestResult.faild;
            forumInfo.header.statusMsg = "页面请求参数错误";
            context.Response.Write( JsonConvert.SerializeObject(forumInfo.header));
        }  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}