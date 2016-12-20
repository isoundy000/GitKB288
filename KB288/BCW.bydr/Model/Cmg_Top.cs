using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// ʵ����Cmg_Top ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Cmg_Top
    {
        public Cmg_Top()
        { }
        #region Model
        private int _id;
        private long _mcolletgold;
        private long _ycolletgold;
        private long _allcolletgold;
        private long _dcolletgold;
        private int _usid;
        private string _changj;
        private long _colletgold;
        private DateTime? _time;
        private int? _bid;
        private int _jid;
        private int _randnum;
        private string _randgoldnum;
        private string _randdaoju;
        private string _randyuID;
        private DateTime _updatetime;
        private string _randten;
        private int _Expiry;
        private int _isrobot;//�۹��� 20160814 ���ӻ�������ע��ʶ
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
        public long McolletGold
        {
            set { _mcolletgold = value; }
            get { return _mcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long YcolletGold
        {
            set { _ycolletgold = value; }
            get { return _ycolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AllcolletGold
        {
            set { _allcolletgold = value; }
            get { return _allcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long DcolletGold
        {
            set { _dcolletgold = value; }
            get { return _dcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int usID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Changj
        {
            set { _changj = value; }
            get { return _changj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long ColletGold
        {
            set { _colletgold = value; }
            get { return _colletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Bid
        {
            set { _bid = value; }
            get { return _bid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int jID
        {
            set { _jid = value; }
            get { return _jid; }
        }
        /// <summary>
        /// 30��һ�ֻ�
        /// </summary>
        public int randnum
        {
            set { _randnum = value; }
            get { return _randnum; }
        }
        /// <summary>
        /// ������ظ�0-29����
        /// </summary>
        public string randgoldnum
        {
            set { _randgoldnum = value; }
            get { return _randgoldnum; }
        }
        /// <summary>
        /// ��ļ۸�
        /// </summary>
        public string randdaoju
        {
            set { _randdaoju = value; }
            get { return _randdaoju; }
        }
        /// <summary>
        /// ���id
        /// </summary>
        public string randyuID
        {
            set { _randyuID = value; }
            get { return _randyuID; }
        }
        /// <summary>
        /// ��ˢʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        ///���10�����ظ�����
        /// </summary>
        public string randten
        {
            set { _randten = value; }
            get { return _randten; }
        }

        /// <summary>
        ///�ظ��ҽ���ʶ
        /// </summary>
        public int Expiry
        {
            set { _Expiry = value; }
            get { return _Expiry; }
        }
        /// <summary>
		/// 0��Ա1������
		/// </summary>
		public int isrobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
        #endregion Model

    }
}

