using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����tb_numsManage ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class numsManage
    {
        public numsManage()
        { }
        #region Model
        private int _id;
        private DateTime _logintime;
        private int _usid;
        private string _pwd;
        private string _question;
        private string _answer;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime loginTime
        {
            set { _logintime = value; }
            get { return _logintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pwd
        {
            set { _pwd = value; }
            get { return _pwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Question
        {
            set { _question = value; }
            get { return _question; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string answer
        {
            set { _answer = value; }
            get { return _answer; }
        }
        #endregion Model

    }
}

