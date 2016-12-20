using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratRoomDo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratRoomDo
	{
		public BaccaratRoomDo()
		{}
		#region Model
		private int _roomdoid;
		private int _usid;
		private string _usname;
		private int _roomid;
		private string _roomdoname;
		private int _roomdotable;
		private int _roomdototal;
		private int _roomdoadd;
		private DateTime _roomdoaddtime;
		private string _roomdotitle;
		private string _roomdoannouces;
		private int _roomdohigh;
		private int _roomdolow;
		private DateTime _roomdoend;
		private int _state;
		/// <summary>
		/// 
		/// </summary>
		public int RoomDoID
		{
			set{ _roomdoid=value;}
			get{return _roomdoid;}
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
			set{ _roomdototal=value;}
			get{return _roomdototal;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomDoAdd
		{
			set{ _roomdoadd=value;}
			get{return _roomdoadd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RoomDoAddTime
		{
			set{ _roomdoaddtime=value;}
			get{return _roomdoaddtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoomDoTitle
		{
			set{ _roomdotitle=value;}
			get{return _roomdotitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoomDoAnnouces
		{
			set{ _roomdoannouces=value;}
			get{return _roomdoannouces;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomDoHigh
		{
			set{ _roomdohigh=value;}
			get{return _roomdohigh;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomDoLow
		{
			set{ _roomdolow=value;}
			get{return _roomdolow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RoomDoEnd
		{
			set{ _roomdoend=value;}
			get{return _roomdoend;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

