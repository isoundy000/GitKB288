﻿using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.BBS.Thread;

namespace BCW.Mobile.Protocol
{
    /// <summary>
    /// 收藏内容请求
    /// </summary>
    public class ReqFavoritesList:ReqProtocolBase
    {
        public int favoritesId;         //收藏Id
    }

    public class FavoritesData
    {
        public int favoritesId;
        public EssencePostItem threadItem;

        public FavoritesData()
        {
            threadItem = new EssencePostItem();
        }
    }

    /// <summary>
    /// 返回收葳内容请求
    /// </summary>
    public class RspFavoritesList : RspProtocolBase
    {
        public List<FavoritesData> lstFavorites;         //已经收藏的帖子列表
        public bool isFinish;                             //是否到底
        public long serverTime;                         //服务器时间

        public RspFavoritesList()
        {
            lstFavorites = new List<FavoritesData>();
        }

    }


    public class ReqAddFavorites : ReqProtocolBase
    {
        public int threadId;     //帖子ID
    }

    public class RspAddFavorites : RspProtocolBase
    {
        public int threadId;        //帖子ID
        public bool isFavorites;    //收藏状态
    }

    public class ReqDelFavorites : ReqProtocolBase
    {
        public int threadId;     //帖子ID
    }

    public class RspDelFavorites : RspProtocolBase
    {
        public int threadId;        //帖子ID
        public bool isFavorites;    //收藏状态
    }
}
