using System;
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


    /// <summary>
    /// 返回收葳内容请求
    /// </summary>
    public class RspFavoritesList : RspProtocolBase
    {
        public List<EssencePostItem> lstThread;         //已经收藏的帖子列表
        public bool isFinish;                             //是否到底
        public long serverTime;                         //服务器时间

        public RspFavoritesList()
        {
            lstThread = new List<EssencePostItem>();    
        }
    }
}
