<%@ WebHandler Language="C#" Class="text" %>

using System;
using System.Web;
using BCW.Mobile.Home;
using BCW.Common;
using BCW.Mobile.BBS;
using BCW.Mobile;

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

            forumInfo.header.status = ERequestResult.eSuccess;
            forumInfo.header.statusMsg = "";

            //是否请求轮播
            string _reqSlider = Utils.GetRequest( "pReqSlider", "all", 1, "", "" );
            if( _reqSlider == "true" )
                forumInfo.InitSlider();

            //是否请求喇叭公告
            string _reqNotice = Utils.GetRequest( "pReqNotice", "all", 1, "", "" );
            if( _reqNotice == "true" )
                forumInfo.InitNotice();

            context.Response.Write( forumInfo.OutPutJsonStr() );

        }
        else
        {
            forumInfo.header.status = ERequestResult.eFail;
            forumInfo.header.statusMsg = "页面请求参数错误";
            context.Response.Write( forumInfo.header.OutPutJsonStr() );
        }  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}