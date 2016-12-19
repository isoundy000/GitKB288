<%@ WebHandler Language="C#" Class="Favorites" %>

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
using BCW.Mobile.Favorites;

public class Favorites : IHttpHandler {

    private HttpContext httpContext;

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        httpContext = context;


        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );


        switch( pAct )
        {
            case "list":                //获取收藏数据
                GetFavoritesData();
                break;
            case "add":                //添加收藏
                AddFavoritesData();
                break;
            case "del":                 //删除收藏
                DelFavoritesData();
                break;
        }


    }

    private void GetFavoritesData()
    {
        ReqFavoritesList _reqData = new ReqFavoritesList();
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.favoritesId = int.Parse(Utils.GetRequest("pFavoritesId", "post", 1, @"^\d*$", "-1"));

        RspFavoritesList _rspData = FavoritesManager.Instance().GetFavoritesList(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void AddFavoritesData()
    {
        ReqAddFavorites _reqData = new ReqAddFavorites();
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));

        RspAddFavorites _rspData = FavoritesManager.Instance().AddFavorites(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());
    }

    private void DelFavoritesData()
    {
        ReqDelFavorites _reqData = new ReqDelFavorites();
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));

        RspDelFavorites _rspData = FavoritesManager.Instance().DelFavorites(_reqData);
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

