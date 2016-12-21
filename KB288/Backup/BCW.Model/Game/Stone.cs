using System;
namespace BCW.Model.Game
{
    /// <summary>
    /// 实体类Stone 。(属性说明自动提取数据库字段的描述信息)
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
        /// 自增ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 游戏类型
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 房间名称
        /// </summary>
        public string StName
        {
            set { _stname = value; }
            get { return _stname; }
        }
        /// <summary>
        /// 下注币额
        /// </summary>
        public int PayCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// 第一个用户
        /// </summary>
        public string OneUsName
        {
            set { _oneusname = value; }
            get { return _oneusname; }
        }
        /// <summary>
        /// 第一用户ID
        /// </summary>
        public int OneUsId
        {
            set { _oneusid = value; }
            get { return _oneusid; }
        }
        /// <summary>
        /// 第二个用户
        /// </summary>
        public string TwoUsName
        {
            set { _twousname = value; }
            get { return _twousname; }
        }
        /// <summary>
        /// 第二个用户ID
        /// </summary>
        public int TwoUsId
        {
            set { _twousid = value; }
            get { return _twousid; }
        }
        /// <summary>
        /// 第三个用户
        /// </summary>
        public string ThrUsName
        {
            set { _thrusname = value; }
            get { return _thrusname; }
        }
        /// <summary>
        /// 第三用户ID
        /// </summary>
        public int ThrUsId
        {
            set { _thrusid = value; }
            get { return _thrusid; }
        }
        /// <summary>
        /// 操作超时时间
        /// </summary>
        public int Expir
        {
            set { _expir = value; }
            get { return _expir; }
        }
        /// <summary>
        /// 用户1出手类型（1剪刀/2石头/3布，下同）
        /// </summary>
        public int OneShot
        {
            set { _oneshot = value; }
            get { return _oneshot; }
        }
        /// <summary>
        /// 用户2出手类型
        /// </summary>
        public int TwoShot
        {
            set { _twoshot = value; }
            get { return _twoshot; }
        }
        /// <summary>
        /// 用户3出手类型
        /// </summary>
        public int ThrShot
        {
            set { _thrshot = value; }
            get { return _thrshot; }
        }
        /// <summary>
        /// 用户1出手时间
        /// </summary>
        public DateTime? OneTime
        {
            set { _onetime = value; }
            get { return _onetime; }
        }
        /// <summary>
        /// 用户2出手时间
        /// </summary>
        public DateTime? TwoTime
        {
            set { _twotime = value; }
            get { return _twotime; }
        }
        /// <summary>
        /// 用户3出手时间
        /// </summary>
        public DateTime? ThrTime
        {
            set { _thrtime = value; }
            get { return _thrtime; }
        }
        /// <summary>
        /// 局数
        /// </summary>
        public int PkCount
        {
            set { _pkcount = value; }
            get { return _pkcount; }
        }

        /// <summary>
        /// 在线人数
        /// </summary>
        public int Online
        {
            set { _online = value; }
            get { return _online; }
        }
        /// <summary>
        /// 最小下注额
        /// </summary>
        public int SmallPay
        {
            set { _smallpay = value; }
            get { return _smallpay; }
        }
        /// <summary>
        /// 最大下注额
        /// </summary>
        public int BigPay
        {
            set { _bigpay = value; }
            get { return _bigpay; }
        }
        /// <summary>
        /// 出手类型（0自由出手/1轮流出手）
        /// </summary>
        public int ShotTypes
        {
            set { _shottypes = value; }
            get { return _shottypes; }
        }
        /// <summary>
        /// 用户1是否在PK状态（0不在/1在PK，下同）
        /// </summary>
        public int OneStat
        {
            set { _onestat = value; }
            get { return _onestat; }
        }
        /// <summary>
        /// 用户2是否在PK状态
        /// </summary>
        public int TwoStat
        {
            set { _twostat = value; }
            get { return _twostat; }
        }
        /// <summary>
        /// 用户3是否在PK状态
        /// </summary>
        public int ThrStat
        {
            set { _thrstat = value; }
            get { return _thrstat; }
        }
        /// <summary>
        /// 是否进行PK（0/非/1是）
        /// </summary>
        public int IsStatus
        {
            set { _isstatus = value; }
            get { return _isstatus; }
        }
        /// <summary>
        /// 下一出手桌子
        /// </summary>
        public int NextShot
        {
            set { _nextshot = value; }
            get { return _nextshot; }
        }
        /// <summary>
        /// 用户在线统计
        /// </summary>
        public string Lines
        {
            set { _lines = value; }
            get { return _lines; }
        }
        #endregion Model

    }
}

