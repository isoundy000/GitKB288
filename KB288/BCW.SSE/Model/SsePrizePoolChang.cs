using System;
namespace BCW.SSE.Model
{
	/// <summary>
	/// SsePrizePoolChang:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SsePrizePoolChang
	{
		public SsePrizePoolChang()
		{}
		#region Model
		private int _id;
		private int _pooltype;
		private int _orderid;
		private int _opertype;
		private int _operid;
		private DateTime _changetime;
		private decimal _changemoney;
		private decimal _totalmoney;
		private string _bz;
		private int _sseno;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int poolType
		{
			set{ _pooltype=value;}
			get{return _pooltype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int orderId
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int operType
		{
			set{ _opertype=value;}
			get{return _opertype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OperId
		{
			set{ _operid=value;}
			get{return _operid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime changeTime
		{
			set{ _changetime=value;}
			get{return _changetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal changeMoney
		{
			set{ _changemoney=value;}
			get{return _changemoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal totalMoney
		{
			set{ _totalmoney=value;}
			get{return _totalmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bz
		{
			set{ _bz=value;}
			get{return _bz;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int sseNo
		{
			set{ _sseno=value;}
			get{return _sseno;}
		}
		#endregion Model

	}
}

