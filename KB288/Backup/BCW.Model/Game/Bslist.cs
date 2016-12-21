using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Bslist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Bslist
	{
		public Bslist()
		{}
		#region Model
		private int _id;
		private string _title;
		private long _money;
		private long _smallpay;
		private long _bigpay;
		private int _usid;
		private string _usname;
		private int _click;
		private int _bztype;
		private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 资金数
		/// </summary>
		public long Money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// 最小下注
		/// </summary>
		public long SmallPay
		{
			set{ _smallpay=value;}
			get{return _smallpay;}
		}
		/// <summary>
		/// 最大下注
		/// </summary>
		public long BigPay
		{
			set{ _bigpay=value;}
			get{return _bigpay;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 人气
		/// </summary>
		public int Click
		{
			set{ _click=value;}
			get{return _click;}
		}
		/// <summary>
		/// 币种类型（0虚拟币/1元）
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

