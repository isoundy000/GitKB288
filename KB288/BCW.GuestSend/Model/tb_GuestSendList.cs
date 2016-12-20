using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_GuestSendList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_GuestSendList
	{
		public tb_GuestSendList()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private int? _guestid;
		private int? _guestsendid;
		private int? _getgold;
		private int? _type;
		private string _remark;
		private int? _iddone;
		private DateTime? _addtime;
		private DateTime? _overtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? guestID
		{
			set{ _guestid=value;}
			get{return _guestid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? guestsendID
		{
			set{ _guestsendid=value;}
			get{return _guestsendid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? getGold
		{
			set{ _getgold=value;}
			get{return _getgold;}
		}
		/// <summary>
		/// 0已领 1已过期 1未阅读
		/// </summary>
		public int? type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? idDone
		{
			set{ _iddone=value;}
			get{return _iddone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		#endregion Model

	}
}

