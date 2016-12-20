using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratCard 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratCard
	{
        public BaccaratCard()
        { }
        #region Model
        private int _id;
        private int _roomid;
        private string _roomdoname;
        private int _roomdotable;
        private string _bankerpoker;
        private int _bankerpoint;
        private string _hunterpoker;
        private int _hunterpoint;
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
        #endregion Model

	}
}

