using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Home;

namespace BCW.Mobile.Protocol
{

    /// <summary>
    /// 请求发表新贴
    /// </summary>
    public class ReqAddThread:ProtocolBase
    {
        public int userId;
        public string userKey;
        public int forumId;
        public int pType;
        public string title;
        public string content;

    }

    /// <summary>
    /// 发表帖子返回
    /// </summary>
    public class RspAddThread : ProtocolBase
    {
        public Header header;
        /// <summary>
        /// 返回的贴子ID
        /// </summary>
        public int threadId;

        public RspAddThread()
        {
            header = new Header();
        }
    }

    /// <summary>
    /// 编辑帖子请求
    /// </summary>
    public class ReqEditThread : ProtocolBase
    {
        public int userId;       //用户ID
        public string userKey;   //用户密钥
        public int threadId;     //帖子ID
        public string title;     //帖子标题
        public string content;   //帖子内容
    }

    public class RspEditThread : RspAddThread
    {

    }

    /// <summary>
    /// 删除帖子
    /// </summary>
    public class ReqDelThread : ProtocolBase
    {
        public int userId;       //用户ID
        public string userKey;   //用户密钥
        public int threadId;     //帖子ID
    }

    public class RspDelThread : RspAddThread
    {

    }
}
