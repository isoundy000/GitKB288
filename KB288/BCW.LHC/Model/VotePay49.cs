using System;
namespace LHC.Model
{
    /// <summary>
    /// ʵ����VotePay49 ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class VotePay49
    {
        public VotePay49()
        { }
        #region Model
        private int _id;
        private int _types;
        private int _qino;
        private int _usid;
        private string _usname;
        private string _vote;
        private long _paycent;
        private long _wincent;
        private int _state;
        private DateTime _addtime;
        private int _bztype;
        private int _payNum;
        /// <summary>
        /// ����ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ��ע��
        /// </summary>
        public int PayNum
        {
            set { _payNum = value; }
            get { return _payNum; }
        }
        /// <summary>
        /// Ͷע����
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int qiNo
        {
            set { _qino = value; }
            get { return _qino; }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// �û��ǳ�
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// Ͷע����
        /// </summary>
        public string Vote
        {
            set { _vote = value; }
            get { return _vote; }
        }
        /// <summary>
        /// ��ע��
        /// </summary>
        public long payCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// Ӯ�Ҷ�
        /// </summary>
        public long winCent
        {
            set { _wincent = value; }
            get { return _wincent; }
        }
        /// <summary>
        /// ״̬��0δ����/1�ѿ���/2�Ѷҽ���
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// Ͷעʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// ����(0�����/1����Ԫ)
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        #endregion Model

    }
}

