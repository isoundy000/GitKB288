using System;
namespace BCW.Model
{
    /// <summary>
    /// ʵ����Topics ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Topics
    {
        public Topics()
        { }
        #region Model
        private int _id;
        private int _leibie;
        private int _nodeid;
        private int _types;
        private string _title;
        private string _content;
        private int _isbr;
        private int _ispc;
        private int _cent;
        private int _selltypes;
        private string _inpwd;
        private string _payid;
        private int _bztype;
        private int _vipleven;
        private int _paixu;
        private int _hidden;
        /// <summary>
        /// ��ĿID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// �ڵ�ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Leibie
        {
            set { _leibie = value; }
            get { return _leibie; }
        }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        public int IsBr
        {
            set { _isbr = value; }
            get { return _isbr; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int IsPc
        {
            set { _ispc = value; }
            get { return _ispc; }
        }
        /// <summary>
        /// �����շѶ���
        /// </summary>
        public int Cent
        {
            set { _cent = value; }
            get { return _cent; }
        }
        /// <summary>
        /// ����ʽ
        /// </summary>
        public int SellTypes
        {
            set { _selltypes = value; }
            get { return _selltypes; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string InPwd
        {
            set { _inpwd = value; }
            get { return _inpwd; }
        }
        /// <summary>
        /// ���μƷ�ʱ�Ĺ���ID
        /// </summary>
        public string PayId
        {
            set { _payid = value; }
            get { return _payid; }
        }
        /// <summary>
        /// �շѱ���
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// ����VIP�ȼ�
        /// </summary>
        public int VipLeven
        {
            set { _vipleven = value; }
            get { return _vipleven; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Paixu
        {
            set { _paixu = value; }
            get { return _paixu; }
        }
        /// <summary>
        /// 0����/1��¼�ɼ�/2����
        /// </summary>
        public int Hidden
        {
            set { _hidden = value; }
            get { return _hidden; }
        }
        #endregion Model

    }
}

