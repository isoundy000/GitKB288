using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Protocol
{
    /// <summary>
    /// 请求签到
    /// </summary>
    public class ReqSignin : ReqProtocolBase
    {
        
    }

    /// <summary>
    /// 发表帖子返回
    /// </summary>
    public class RspSignin : RspProtocolBase
    {
        /// <summary>
        /// 签到总天数
        /// </summary>
        public int totalDay;

        /// <summary>
        /// 连续签到天数
        /// </summary>
        public int keepDay;

        public string signinRewardStr;

        /// <summary>
        /// 连续1个月签到奖励积分
        /// </summary>
        public string monthRewardStr;

        /// <summary>
        /// 连续一周签到奖励积分
        /// </summary>
        public string weekRewardStr;


    }
}
