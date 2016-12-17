using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Protocol
{

    /// <summary>
    /// 评论(回帖)列表请求
    /// </summary>
    public class ReqReplyList : ReqProtocolBase
    {
        public int threadId;     //帖子ID   

        /// <summary>
        /// 返回类型(0:该帖所有评论  1:该帖精华评论  2：该帖置顶评论  3:该帖楼主评论  3:该帖热门评论(暂未研发) )
        /// </summary>
        public int showType;

        /// <summary>
        /// 某用户ID
        /// </summary>
        public int authorId;

        /// <summary>
        /// 评论ID（分页用）
        /// </summary>
        public int replyId;
    }


    public class RspReplyData
    {
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool isTop;

        /// <summary>
        /// 是否精华
        /// </summary>
        public bool isGood;

        /// <summary>
        /// 是否附件帖
        /// </summary>
        public bool isFile;

        /// <summary>
        /// 回贴楼数
        /// </summary>
        public int floor;

        /// <summary>
        /// 回帖内容
        /// </summary>
        public string content;

        /// <summary>
        /// 回复楼层ID（0：回复的是主题帖 ）
        /// </summary>
        public int ReplyId;

        /// <summary>
        /// 被回复楼层的原评论内容
        /// </summary>
        public string replyContent;

        /// <summary>
        /// 发表时间
        /// </summary>
        public long addTime;

        /// <summary>
        /// 点赞数
        /// </summary>
        public int praise;
    }

    /// <summary>
    /// 评论(回帖)列表请求返回
    /// </summary>
    public class RspReplyList : RspProtocolBase
    {
        public List<RspReplyData> lstReplyData;

        public RspReplyList():base()
        {
            lstReplyData = new List<RspReplyData>();
        }
    }

    /// <summary>
    /// 评论请求
    /// </summary>
    public class ReqAddReplyThread : ReqProtocolBase
    {
        public int threadId;     //帖子ID

    }

    /// <summary>
    /// 评论请求返回
    /// </summary>
    public class RspAddReplyThread : RspProtocolBase
    {

    }
}
