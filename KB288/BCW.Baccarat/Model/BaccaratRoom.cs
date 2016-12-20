using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BaccaratRoom 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BaccaratRoom
	{
		public BaccaratRoom()
		{}
		#region Model
		private int _roomid;
		private int _usid;
		private string _usname;
		private int _roomtotal;
		private int _roomhigh;
		private int _roomlow;
		private DateTime _roomstart;
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
		public int RoomTotal
		{
			set{ _roomtotal=value;}
			get{return _roomtotal;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomHigh
		{
			set{ _roomhigh=value;}
			get{return _roomhigh;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RoomLow
		{
			set{ _roomlow=value;}
			get{return _roomlow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RoomStart
		{
			set{ _roomstart=value;}
			get{return _roomstart;}
		}
		#endregion Model

	}
}

