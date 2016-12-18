using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Home;

namespace BCW.Mobile.Protocol
{

    /// <summary>
    /// 请求发表新贴
    /// </summary>
    public class ReqAddThread:ReqProtocolBase
    {
        public int forumId;
        public int pType;
        public string title;
        public string content;

    }

    /// <summary>
    /// 发表帖子返回
    /// </summary>
    public class RspAddThread : RspProtocolBase
    {
        /// <summary>
        /// 返回的贴子ID
        /// </summary>
        public int threadId;

    }

    /// <summary>
    /// 编辑帖子请求
    /// </summary>
    public class ReqEditThread : ReqProtocolBase
    {
        public int threadId;     //帖子ID
        public string title;     //帖子标题
        public string content;   //帖子内容
    }

    public class RspEditThread : RspProtocolBase
    {
        public int threadId;
    }

    /// <summary>
    /// 删除帖子
    /// </summary>
    public class ReqDelThread : ReqProtocolBase
    {
        public int threadId;     //帖子ID
    }

    public class RspDelThread : RspProtocolBase
    {

    }
}
