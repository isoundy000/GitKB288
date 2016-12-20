using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Health 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Health
    {
        public Health()
        { }
        #region Model
        private int _id;
        private int _eventcode;
        private string _message;
        private DateTime _eventtime;
        private string _requesturl;
        private string _exceptiontype;
        private string _exceptionmessage;
        /// <summary>
        /// 事件ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 事件代码
        /// </summary>
        public int EventCode
        {
            set { _eventcode = value; }
            get { return _eventcode; }
        }
        /// <summary>
        ///事件消息
        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventTime
        {
            set { _eventtime = value; }
            get { return _eventtime; }
        }
        /// <summary>
        /// 请求 Url
        /// </summary>
        public string RequestUrl
        {
            set { _requesturl = value; }
            get { return _requesturl; }
        }
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType
        {
            set { _exceptiontype = value; }
            get { return _exceptiontype; }
        }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ExceptionMessage
        {
            set { _exceptionmessage = value; }
            get { return _exceptionmessage; }
        }
        #endregion Model

    }
}
