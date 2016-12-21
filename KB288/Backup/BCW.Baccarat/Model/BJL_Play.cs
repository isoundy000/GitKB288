using System;
namespace BCW.Baccarat.Model
{
    /// <summary>
    /// ʵ����BJL_Play ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class BJL_Play
    {
        public BJL_Play()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _roomid;
        private int _play_table;
        private string _puttypes;
        private string _bankerpoker;
        private int _bankerpoint;
        private string _hunterpoker;
        private int _hunterpoint;
        private DateTime _updatetime;
        private int _isrobot;
        private long _total;
        private int _buy_usid;
        private long _zhu_money;
        private long _putmoney;
        private long _getmoney;
        private int _type;
        private int _shouxufei;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// ����ID
        /// </summary>
        public int RoomID
        {
            set { _roomid = value; }
            get { return _roomid; }
        }
        /// <summary>
        /// �ڼ���
        /// </summary>
        public int Play_Table
        {
            set { _play_table = value; }
            get { return _play_table; }
        }
        /// <summary>
        /// ��ע����
        /// </summary>
        public string PutTypes
        {
            set { _puttypes = value; }
            get { return _puttypes; }
        }
        /// <summary>
        /// ׯ��
        /// </summary>
        public string BankerPoker
        {
            set { _bankerpoker = value; }
            get { return _bankerpoker; }
        }
        /// <summary>
        /// ׯ����
        /// </summary>
        public int BankerPoint
        {
            set { _bankerpoint = value; }
            get { return _bankerpoint; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string HunterPoker
        {
            set { _hunterpoker = value; }
            get { return _hunterpoker; }
        }
        /// <summary>
        /// �е���
        /// </summary>
        public int HunterPoint
        {
            set { _hunterpoint = value; }
            get { return _hunterpoint; }
        }
        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 0��Ա1������
        /// </summary>
        public int isRobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
        /// <summary>
        /// �ʳ�
        /// </summary>
        public long Total
        {
            set { _total = value; }
            get { return _total; }
        }
        /// <summary>
        /// ������û�ID
        /// </summary>
        public int buy_usid
        {
            set { _buy_usid = value; }
            get { return _buy_usid; }
        }
        /// <summary>
        /// ÿע���
        /// </summary>
        public long zhu_money
        {
            set { _zhu_money = value; }
            get { return _zhu_money; }
        }
        /// <summary>
        /// Ͷע���
        /// </summary>
        public long PutMoney
        {
            set { _putmoney = value; }
            get { return _putmoney; }
        }
        /// <summary>
        /// �õ���Ǯ
        /// </summary>
        public long GetMoney
        {
            set { _getmoney = value; }
            get { return _getmoney; }
        }
        /// <summary>
        /// 0δ����1����2��3���콱
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// �õ��н�������
        /// </summary>
        public int shouxufei
        {
            set { _shouxufei = value; }
            get { return _shouxufei; }
        }
        #endregion Model

    }
}

