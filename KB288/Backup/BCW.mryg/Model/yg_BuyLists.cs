using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类yg_BuyLists 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class yg_BuyLists
	{
		public yg_BuyLists()
		{}
		#region Model
		private long _id;
		private string _userid;
		private long _goodsnum;
		private string _yungouma;
		private int? _counts;
		private string _ip;
		private string _system;
		private string _address;
		private DateTime? _buytime;
		private int? _isget;
		/// <summary>
		/// 
		/// </summary>
		public long Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long GoodsNum
		{
			set{ _goodsnum=value;}
			get{return _goodsnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string yungouma
		{
			set{ _yungouma=value;}
			get{return _yungouma;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Counts
		{
			set{ _counts=value;}
			get{return _counts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ip
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string System
		{
			set{ _system=value;}
			get{return _system;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? BuyTime
		{
			set{ _buytime=value;}
			get{return _buytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsGet
		{
			set{ _isget=value;}
			get{return _isget;}
		}
		#endregion Model

	}
}

