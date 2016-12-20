using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratTop 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratTop
	{
		public BaccaratTop()
		{}
		#region Model
		private int _topid;
		private int _usid;
		private string _usname;
		private DateTime _topdate;
		private int _topbonussum;
        private int _aa;
        private int _roomid;
        private int _roomtable;
		/// <summary>
		/// 
		/// </summary>
		public int TopID
		{
			set{ _topid=value;}
			get{return _topid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Topdate
		{
			set{ _topdate=value;}
			get{return _topdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TopBonusSum
		{
			set{ _topbonussum=value;}
			get{return _topbonussum;}
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
        public int RoomTable
        {
            set { _roomtable = value; }
            get { return _roomtable; }
        }
		#endregion Model

	}
}

