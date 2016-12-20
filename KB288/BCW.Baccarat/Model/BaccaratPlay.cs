using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratPlay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratPlay
	{
		public BaccaratPlay()
		{}
		#region Model
        private int _id;
        private int _usid;
        private string _usname;
        private int _roomid;
        private string _roomdoname;
        private int _roomdotable;
        private int _roomdototal;
        private string _roomdotitle;
        private string _roomdoannouces;
        private int _roomdohigh;
        private int _roomdolow;
        private DateTime _settime;
        private int _actid;
        private DateTime _lasttime;
        private int _RoomTime;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        public int RoomTime
        {
            set { _RoomTime = value; }
            get { return _RoomTime; }
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
        public int RoomDoHigh
        {
            set { _roomdohigh = value; }
            get { return _roomdohigh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoomDoLow
        {
            set { _roomdolow = value; }
            get { return _roomdolow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SetTime
        {
            set { _settime = value; }
            get { return _settime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ActID
        {
            set { _actid = value; }
            get { return _actid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastTime
        {
            set { _lasttime = value; }
            get { return _lasttime; }
        }
		#endregion Model

	}
}

