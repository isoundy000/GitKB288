using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratHistory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratHistory
	{
		public BaccaratHistory()
		{}
		#region Model
        private int _hid;
        private int _roomid;
        private string _roomdoname;
        private int _roomdotable;
        private int _usid;
        private string _usname;
        private int _betmoney;
        private string _bettype;
        private string _bankerpoker;
        private int _bankerpoint;
        private string _hunterpoker;
        private int _hunterpoint;
        private string _bonusplayer;
        private int _bonusmoney;
        private DateTime _bonustimes;
        /// <summary>
        /// 
        /// </summary>
        public int HID
        {
            set { _hid = value; }
            get { return _hid; }
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
        public int BetMoney
        {
            set { _betmoney = value; }
            get { return _betmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BetType
        {
            set { _bettype = value; }
            get { return _bettype; }
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
        public string BonusPlayer
        {
            set { _bonusplayer = value; }
            get { return _bonusplayer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BonusMoney
        {
            set { _bonusmoney = value; }
            get { return _bonusmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BonusTimes
        {
            set { _bonustimes = value; }
            get { return _bonustimes; }
        }
		#endregion Model

	}
}

