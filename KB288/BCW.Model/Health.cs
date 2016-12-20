using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Health ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// �¼�ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// �¼�����
        /// </summary>
        public int EventCode
        {
            set { _eventcode = value; }
            get { return _eventcode; }
        }
        /// <summary>
        ///�¼���Ϣ
        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }
        /// <summary>
        /// �¼�ʱ��
        /// </summary>
        public DateTime EventTime
        {
            set { _eventtime = value; }
            get { return _eventtime; }
        }
        /// <summary>
        /// ���� Url
        /// </summary>
        public string RequestUrl
        {
            set { _requesturl = value; }
            get { return _requesturl; }
        }
        /// <summary>
        /// �쳣����
        /// </summary>
        public string ExceptionType
        {
            set { _exceptiontype = value; }
            get { return _exceptiontype; }
        }
        /// <summary>
        /// �쳣��Ϣ
        /// </summary>
        public string ExceptionMessage
        {
            set { _exceptionmessage = value; }
            get { return _exceptionmessage; }
        }
        #endregion Model

    }
}
