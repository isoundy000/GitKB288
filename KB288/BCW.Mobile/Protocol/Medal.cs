using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Protocol
{
    public class ReqMedalLog:ReqProtocolBase
    {
        public int medalId; 
    }

    public class MedalData
    {
        public int userId;
        public string userName;
        public string content;
        public long addTime;
    }


    public class RspMedalLog : RspProtocolBase
    {
        /// <summary>
        /// 是否到底
        /// </summary>
        public bool isFinish = true;

        /// <summary>
        /// 服务器时间
        /// </summary>
        public long serverTime;

        //动态数据列表
        public List<ActionData> lstMedal;

        public RspMedalLog()
        {
            lstMedal = new List<ActionData>();
        }

    }
}
