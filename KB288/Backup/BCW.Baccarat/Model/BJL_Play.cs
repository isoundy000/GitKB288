using System;
namespace BCW.Baccarat.Model
{
    /// <summary>
    /// 实体类BJL_Play 。(属性说明自动提取数据库字段的描述信息)
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
        /// 用户ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 房间ID
        /// </summary>
        public int RoomID
        {
            set { _roomid = value; }
            get { return _roomid; }
        }
        /// <summary>
        /// 第几局
        /// </summary>
        public int Play_Table
        {
            set { _play_table = value; }
            get { return _play_table; }
        }
        /// <summary>
        /// 下注类型
        /// </summary>
        public string PutTypes
        {
            set { _puttypes = value; }
            get { return _puttypes; }
        }
        /// <summary>
        /// 庄牌
        /// </summary>
        public string BankerPoker
        {
            set { _bankerpoker = value; }
            get { return _bankerpoker; }
        }
        /// <summary>
        /// 庄点数
        /// </summary>
        public int BankerPoint
        {
            set { _bankerpoint = value; }
            get { return _bankerpoint; }
        }
        /// <summary>
        /// 闲牌
        /// </summary>
        public string HunterPoker
        {
            set { _hunterpoker = value; }
            get { return _hunterpoker; }
        }
        /// <summary>
        /// 闲点数
        /// </summary>
        public int HunterPoint
        {
            set { _hunterpoint = value; }
            get { return _hunterpoint; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 0会员1机器人
        /// </summary>
        public int isRobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
        /// <summary>
        /// 彩池
        /// </summary>
        public long Total
        {
            set { _total = value; }
            get { return _total; }
        }
        /// <summary>
        /// 购买的用户ID
        /// </summary>
        public int buy_usid
        {
            set { _buy_usid = value; }
            get { return _buy_usid; }
        }
        /// <summary>
        /// 每注金额
        /// </summary>
        public long zhu_money
        {
            set { _zhu_money = value; }
            get { return _zhu_money; }
        }
        /// <summary>
        /// 投注金额
        /// </summary>
        public long PutMoney
        {
            set { _putmoney = value; }
            get { return _putmoney; }
        }
        /// <summary>
        /// 得到的钱
        /// </summary>
        public long GetMoney
        {
            set { _getmoney = value; }
            get { return _getmoney; }
        }
        /// <summary>
        /// 0未开奖1不中2中3已领奖
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 得到中奖的类型
        /// </summary>
        public int shouxufei
        {
            set { _shouxufei = value; }
            get { return _shouxufei; }
        }
        #endregion Model

    }
}

