using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Protocol
{
    public class reqAction : ReqProtocolBase
    {
        public int actionId;
    }


    public class ActionData
    {
        public int actionId;
        public int userId;
        public string userName;
        public string content;
        public long addTime;
    }

    public class rspAction : RspProtocolBase
    {
        /// <summary>
        /// 人气
        /// </summary>
        public string totalClick;

        /// <summary>
        /// 在线时长
        /// </summary>
        public string onLineTime;

        /// <summary>
        /// 是否到底
        /// </summary>
        public bool isFinish = true;

        /// <summary>
        /// 服务器时间
        /// </summary>
        public long serverTime;

        //动态数据列表
        public List<ActionData> lstAction;

        public rspAction()
        {
            lstAction = new List<ActionData>();
        }        
           
    }
}
