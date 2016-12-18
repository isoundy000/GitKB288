<%@ WebHandler Language="C#" Class="home" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class home : IHttpHandler {
    private HomePageInfo homePageInfo;
    private int meid;

    public home()
    {
        homePageInfo = new HomePageInfo(); 
    }
    
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

            
        
        string _act = Utils.GetRequest( "pAct", "all", 1, "", "" );

        if( _act == "list" )
        {
            homePageInfo.header.status = ERequestResult.success;
            homePageInfo.header.statusMsg ="";
            
            //检查是否有内线消息
            homePageInfo.has_msg = new BCW.BLL.Guest().GetXCount( meid ) > 0;
            //是否登录
            homePageInfo.is_login = meid != 0;
          
            //是否请求轮播
            string _reqSlider = Utils.GetRequest( "pReqSlider", "all", 1, "", "" );
            if( _reqSlider == "true" )
                homePageInfo.InitSlider();

            //是否请求喇叭公告
            string _reqNotice = Utils.GetRequest( "pReqNotice", "all", 1, "", "" );
            if( _reqNotice == "true" )
                homePageInfo.InitNotice();

            //是否请求所有精华贴
            string _reqPost = Utils.GetRequest( "pReqPost", "all", 1, "", "" );
            string  _threadId =Utils.GetRequest( "pthreadId", "all", 1, "", "-1" );
            if( _reqPost == "true" )
                homePageInfo.InitEssencePost( _threadId == "" ? -1 : int.Parse(_threadId) );
            
            
            
            //论坛精华       
            context.Response.Write(JsonConvert.SerializeObject(homePageInfo));
        }
        else
        {
            homePageInfo.header.status = ERequestResult.faild;
            homePageInfo.header.statusMsg = "页面请求参数错误";
            context.Response.Write(JsonConvert.SerializeObject(homePageInfo.header)); 
        }      
         
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

