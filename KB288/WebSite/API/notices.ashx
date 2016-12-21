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
using BCW.Mobile.Protocol;

public class notices : IHttpHandler {
    private HttpContext mContext;


    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        mContext = context;

        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" ) ;

        switch (pAct)
        {
            case "list":
                GetNoticeList();
                break;
            case "addSuona":
                AddNotice();
                break;

        }

    }

    /// <summary>
    /// 返回公告数据
    /// </summary>
    private void GetNoticeList()
    {
        RspNoticAll _rspData = new RspNoticAll();
        int pNoticeId = int.Parse( Utils.GetRequest( "pNoticeId", "all", 1, @"^\d*$", "-1" ) );
        int pType = int.Parse( Utils.GetRequest( "pType", "all", 1, @"^\d*$", "0" ) );              //类型
        int pUsId = int.Parse( Utils.GetRequest( "pUserId", "all", 1, @"^\d*$", "-1" ) );
        int pExpire = int.Parse( Utils.GetRequest( "pExpire", "all", 1, @"^\d*$", "-1" ) );        //不填:包含过期和未过期  0：未过期  1：已过期

        switch( (ENoticeType) pType )
        {
            case ENoticeType.e_all:                 //公告与喇叭混合数据
                _rspData.notice = new Notices( ENoticeType.e_all );
                ( ( NoticesAllItem )  _rspData.notice.noticeData ).InitData( pNoticeId );
                break;
            case ENoticeType.e_suona:               //纯喇叭数据
                _rspData.notice = new Notices( ENoticeType.e_suona );
                ( ( NoticesSuonaItem )  _rspData.notice.noticeData ).InitData( pNoticeId, pExpire, pUsId );
                break;
        }

        _rspData.header.status = ERequestResult.success;
        mContext.Response.Write(_rspData.SerializeObject());
    }

    /// <summary>
    /// 发布公告
    /// </summary>
    private void AddNotice()
    {
        ReqAddSuona _suonaData = new ReqAddSuona();
        _suonaData.userId =  int.Parse( Utils.GetRequest( "pUserId", "post", 1, @"^\d*$", "-1" ) );
        _suonaData.userKey = Utils.GetRequest( "pUsKey", "post", 0, "", "" );
        _suonaData.content = Utils.GetRequest("pContent", "post", 0, "", "");
        _suonaData.minute = int.Parse(Utils.GetRequest("pMinute", "post", 1, @"^\d*$", "-1"));

        RspAddSuona _rspData = new RspAddSuona();
        Notices _Notices = new Notices( ENoticeType.e_suona );
        _rspData = ((NoticesSuonaItem)_Notices.noticeData).AddSuonaItem(_suonaData);
        mContext.Response.Write(_rspData.SerializeObject());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}