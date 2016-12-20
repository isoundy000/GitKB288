using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// 实体类Cmg_Top 。(属性说明自动提取数据库字段的描述信息)
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
        private int _isrobot;//邵广林 20160814 增加机器人下注标识
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
        /// 30次一轮回
        /// </summary>
        public int randnum
        {
            set { _randnum = value; }
            get { return _randnum; }
        }
        /// <summary>
        /// 随机不重复0-29的数
        /// </summary>
        public string randgoldnum
        {
            set { _randgoldnum = value; }
            get { return _randgoldnum; }
        }
        /// <summary>
        /// 鱼的价格
        /// </summary>
        public string randdaoju
        {
            set { _randdaoju = value; }
            get { return _randdaoju; }
        }
        /// <summary>
        /// 鱼的id
        /// </summary>
        public string randyuID
        {
            set { _randyuID = value; }
            get { return _randyuID; }
        }
        /// <summary>
        /// 防刷时间
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        ///随机10个不重复的数
        /// </summary>
        public string randten
        {
            set { _randten = value; }
            get { return _randten; }
        }

        /// <summary>
        ///重复兑奖标识
        /// </summary>
        public int Expiry
        {
            set { _Expiry = value; }
            get { return _Expiry; }
        }
        /// <summary>
		/// 0会员1机器人
		/// </summary>
		public int isrobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
        #endregion Model

    }
}

