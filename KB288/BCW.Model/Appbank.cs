using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Appbank 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Appbank
	{
		public Appbank()
		{}
		#region Model
		private int _id;
		private int _types;
		private long _addgold;
		private int _usid;
		private string _usname;
		private string _mobile;
		private string _cardnum;
		private string _cardname;
		private string _cardaddress;
		private string _notes;
		private string _adminnotes;
		private int _state;
		private DateTime _addtime;
		private DateTime _retime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型，默认0
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 申请金额数目
		/// </summary>
		public long AddGold
		{
			set{ _addgold=value;}
			get{return _addgold;}
		}
		/// <summary>
		/// 申请ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 申请昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 卡号
		/// </summary>
		public string CardNum
		{
			set{ _cardnum=value;}
			get{return _cardnum;}
		}
		/// <summary>
		/// 卡姓名
		/// </summary>
		public string CardName
		{
			set{ _cardname=value;}
			get{return _cardname;}
		}
		/// <summary>
		/// 开户地址
		/// </summary>
		public string CardAddress
		{
			set{ _cardaddress=value;}
			get{return _cardaddress;}
		}
		/// <summary>
		/// 申请者备注
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 管理员备注
		/// </summary>
		public string AdminNotes
		{
			set{ _adminnotes=value;}
			get{return _adminnotes;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 申请时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 受理时间
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		#endregion Model

	}
}

