using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Manage ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Manage
    {
        public Manage()
        { }
        #region Model
        private int _id;
        private string _suser;
        private string _spwd;
        private string _skeys;
        private DateTime _stime;
        private string _suserip;
        /// <summary>
        /// ����ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public string sUser
        {
            set { _suser = value; }
            get { return _suser; }
        }
        /// <summary>
        /// �û�����
        /// </summary>
        public string sPwd
        {
            set { _spwd = value; }
            get { return _spwd; }
        }
        /// <summary>
        /// ���������
        /// </summary>
        public string sKeys
        {
            set { _skeys = value; }
            get { return _skeys; }
        }
        /// <summary>
        /// ��¼ʱ��
        /// </summary>
        public DateTime sTime
        {
            set { _stime = value; }
            get { return _stime; }
        }
        /// <summary>
        /// ��¼IP
        /// </summary>
        public string sUserIP
        {
            set { _suserip = value; }
            get { return _suserip; }
        }
        #endregion Model

    }
}
