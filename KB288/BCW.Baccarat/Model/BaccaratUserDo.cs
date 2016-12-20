using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratUserDo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratUserDo
	{
		public BaccaratUserDo()
		{}
		#region Model
		private int _doid;
		private int _usid;
		private string _usname;
		private int _roomid;
		private string _roomdoname;
		private int _roomdotable;
        private int _roomdototal;
		private int _betmoney;
		private string _bettypes;
		private DateTime _betdate;
		private int _bonusmoney;
		private DateTime _bonustimes;
		private int _type;
		/// <summary>
		/// 
		/// </summary>
		public int DoID
		{
			set{ _doid=value;}
			get{return _doid;}
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
		public int RoomID
		{
			set{ _roomid=value;}
			get{return _roomid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoomDoName
		{
			set{ _roomdoname=value;}
			get{return _roomdoname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomDoTable
		{
			set{ _roomdotable=value;}
			get{return _roomdotable;}
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
		public int BetMoney
		{
			set{ _betmoney=value;}
			get{return _betmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BetTypes
		{
			set{ _bettypes=value;}
			get{return _bettypes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime BetDate
		{
			set{ _betdate=value;}
			get{return _betdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BonusMoney
		{
			set{ _bonusmoney=value;}
			get{return _bonusmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime BonusTimes
		{
			set{ _bonustimes=value;}
			get{return _bonustimes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

