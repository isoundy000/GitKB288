using System;
namespace BCW.Model.Game
{
    /// <summary>
    /// ʵ����Stone ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class Stone
    {
        public Stone()
        { }
        #region Model
        private int _id;
        private int _types;
        private string _stname;
        private int _paycent;
        private string _oneusname;
        private int _oneusid;
        private string _twousname;
        private int _twousid;
        private string _thrusname;
        private int _thrusid;
        private int _expir;
        private int _oneshot;
        private int _twoshot;
        private int _thrshot;
        private DateTime? _onetime;
        private DateTime? _twotime;
        private DateTime? _thrtime;
        private int _pkcount;
        private int _online;
        private int _smallpay;
        private int _bigpay;
        private int _shottypes;
        private int _onestat;
        private int _twostat;
        private int _thrstat;
        private int _isstatus;
        private int _nextshot;
        private string _lines;
        /// <summary>
        /// ����ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ��Ϸ����
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string StName
        {
            set { _stname = value; }
            get { return _stname; }
        }
        /// <summary>
        /// ��ע�Ҷ�
        /// </summary>
        public int PayCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// ��һ���û�
        /// </summary>
        public string OneUsName
        {
            set { _oneusname = value; }
            get { return _oneusname; }
        }
        /// <summary>
        /// ��һ�û�ID
        /// </summary>
        public int OneUsId
        {
            set { _oneusid = value; }
            get { return _oneusid; }
        }
        /// <summary>
        /// �ڶ����û�
        /// </summary>
        public string TwoUsName
        {
            set { _twousname = value; }
            get { return _twousname; }
        }
        /// <summary>
        /// �ڶ����û�ID
        /// </summary>
        public int TwoUsId
        {
            set { _twousid = value; }
            get { return _twousid; }
        }
        /// <summary>
        /// �������û�
        /// </summary>
        public string ThrUsName
        {
            set { _thrusname = value; }
            get { return _thrusname; }
        }
        /// <summary>
        /// �����û�ID
        /// </summary>
        public int ThrUsId
        {
            set { _thrusid = value; }
            get { return _thrusid; }
        }
        /// <summary>
        /// ������ʱʱ��
        /// </summary>
        public int Expir
        {
            set { _expir = value; }
            get { return _expir; }
        }
        /// <summary>
        /// �û�1�������ͣ�1����/2ʯͷ/3������ͬ��
        /// </summary>
        public int OneShot
        {
            set { _oneshot = value; }
            get { return _oneshot; }
        }
        /// <summary>
        /// �û�2��������
        /// </summary>
        public int TwoShot
        {
            set { _twoshot = value; }
            get { return _twoshot; }
        }
        /// <summary>
        /// �û�3��������
        /// </summary>
        public int ThrShot
        {
            set { _thrshot = value; }
            get { return _thrshot; }
        }
        /// <summary>
        /// �û�1����ʱ��
        /// </summary>
        public DateTime? OneTime
        {
            set { _onetime = value; }
            get { return _onetime; }
        }
        /// <summary>
        /// �û�2����ʱ��
        /// </summary>
        public DateTime? TwoTime
        {
            set { _twotime = value; }
            get { return _twotime; }
        }
        /// <summary>
        /// �û�3����ʱ��
        /// </summary>
        public DateTime? ThrTime
        {
            set { _thrtime = value; }
            get { return _thrtime; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int PkCount
        {
            set { _pkcount = value; }
            get { return _pkcount; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public int Online
        {
            set { _online = value; }
            get { return _online; }
        }
        /// <summary>
        /// ��С��ע��
        /// </summary>
        public int SmallPay
        {
            set { _smallpay = value; }
            get { return _smallpay; }
        }
        /// <summary>
        /// �����ע��
        /// </summary>
        public int BigPay
        {
            set { _bigpay = value; }
            get { return _bigpay; }
        }
        /// <summary>
        /// �������ͣ�0���ɳ���/1�������֣�
        /// </summary>
        public int ShotTypes
        {
            set { _shottypes = value; }
            get { return _shottypes; }
        }
        /// <summary>
        /// �û�1�Ƿ���PK״̬��0����/1��PK����ͬ��
        /// </summary>
        public int OneStat
        {
            set { _onestat = value; }
            get { return _onestat; }
        }
        /// <summary>
        /// �û�2�Ƿ���PK״̬
        /// </summary>
        public int TwoStat
        {
            set { _twostat = value; }
            get { return _twostat; }
        }
        /// <summary>
        /// �û�3�Ƿ���PK״̬
        /// </summary>
        public int ThrStat
        {
            set { _thrstat = value; }
            get { return _thrstat; }
        }
        /// <summary>
        /// �Ƿ����PK��0/��/1�ǣ�
        /// </summary>
        public int IsStatus
        {
            set { _isstatus = value; }
            get { return _isstatus; }
        }
        /// <summary>
        /// ��һ��������
        /// </summary>
        public int NextShot
        {
            set { _nextshot = value; }
            get { return _nextshot; }
        }
        /// <summary>
        /// �û�����ͳ��
        /// </summary>
        public string Lines
        {
            set { _lines = value; }
            get { return _lines; }
        }
        #endregion Model

    }
}

