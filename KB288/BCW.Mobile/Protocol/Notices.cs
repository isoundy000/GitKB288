using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Protocol
{
    /// <summary>
    /// 请求发表新喇叭
    /// </summary>
    public class ReqAddSuona : ReqProtocolBase
    {
        public string content;
        public int minute;
    }

    /// <summary>
    /// 发表帖子返回
    /// </summary>
    public class RspAddSuona : RspProtocolBase
    {
        /// <summary>
        /// 返回的贴子ID
        /// </summary>
        public int suonaId;

        /// <summary>
        /// 消耗金钱
        /// </summary>
        public int cobi;

    }

    public class RspNoticAll : RspProtocolBase
    {
        public Notices notice;
    }
}
