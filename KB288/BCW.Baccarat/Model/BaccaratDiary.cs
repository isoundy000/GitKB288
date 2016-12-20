using System;
namespace BCW.Baccarat.Model
{
    /// <summary>
    /// BaccaratDiary:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BaccaratDiary
    {
        public BaccaratDiary()
        { }
        #region Model
        private int _id;
        private int _usid;
        private string _usname;
        private int _roomid;
        private string _roomdoname;
        private int _roomdotable;
        private int _roomdototal;
        private int _roomdoadd;
        private string _roomdotitle;
        private string _roomdoannouces;
        private int _betmoney;
        private string _bettypes;
        private string _bankerpoker;
        private int _bankerpoint;
        private string _hunterpoker;
        private int _hunterpoint;
        private int _actid;
        private DateTime _updatetime;
        private int _bonusmoney; 
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
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UsName
        {
            set { _usname = value; }
            get { return _usname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoomID
        {
            set { _roomid = value; }
            get { return _roomid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoomDoName
        {
            set { _roomdoname = value; }
            get { return _roomdoname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoomDoTable
        {
            set { _roomdotable = value; }
            get { return _roomdotable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoomDoTotal
        {
            set { _roomdototal = value; }
            get { return _roomdototal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoomDoAdd
        {
            set { _roomdoadd = value; }
            get { return _roomdoadd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoomDoTitle
        {
            set { _roomdotitle = value; }
            get { return _roomdotitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoomDoAnnouces
        {
            set { _roomdoannouces = value; }
            get { return _roomdoannouces; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BetMoney
        {
            set { _betmoney = value; }
            get { return _betmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BetTypes
        {
            set { _bettypes = value; }
            get { return _bettypes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BankerPoker
        {
            set { _bankerpoker = value; }
            get { return _bankerpoker; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BankerPoint
        {
            set { _bankerpoint = value; }
            get { return _bankerpoint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HunterPoker
        {
            set { _hunterpoker = value; }
            get { return _hunterpoker; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int HunterPoint
        {
            set { _hunterpoint = value; }
            get { return _hunterpoint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int actid
        {
            set { _actid = value; }
            get { return _actid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        public int BonusMoney
        {
            set { _bonusmoney = value; }
            get { return _bonusmoney; }
        }
        #endregion Model

    }
}