<%@ WebHandler Language="C#" Class="notices" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class notices : IHttpHandler {
    private Notices mNotices;    
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int pNoticeId = int.Parse( Utils.GetRequest( "pNoticeId", "all", 1, @"^\d*$", "-1" ) );
        int pType = int.Parse( Utils.GetRequest( "pType", "all", 1, @"^\d*$", "0" ) );              //类型
        int pUsId = int.Parse( Utils.GetRequest( "pUsId", "all", 1, @"^\d*$", "-1" ) );
        int pExpire = int.Parse( Utils.GetRequest( "pExpire", "all", 1, @"^\d*$", "0" ) );

        switch( (ENoticeType) pType )
        {
            case ENoticeType.e_all:                 //公告与喇叭混合数据
                mNotices = new Notices( ENoticeType.e_all );
                ( ( NoticesAllItem ) mNotices.noticeData ).InitData( pNoticeId );
                break;
            case ENoticeType.e_suona:               //纯喇叭数据
                mNotices = new Notices( ENoticeType.e_suona );
                ( ( NoticesSuonaItem ) mNotices.noticeData ).InitData( pNoticeId, pExpire, pUsId );
                break;
        }
       
        context.Response.Write(JsonConvert.SerializeObject(mNotices));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}