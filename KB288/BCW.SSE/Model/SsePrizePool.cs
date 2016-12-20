using System;
namespace BCW.SSE.Model
{
	/// <summary>
	/// SsePrizePool:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SsePrizePool
	{
		public SsePrizePool()
		{}
		#region Model
		private int _id;
		private int _prizetype=0;
		private int _sseno;
		private decimal _totalprizemoney=0M;
		private string _bz;
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
		public int prizeType
		{
			set{ _prizetype=value;}
			get{return _prizetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int sseNo
		{
			set{ _sseno=value;}
			get{return _sseno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal totalPrizeMoney
		{
			set{ _totalprizemoney=value;}
			get{return _totalprizemoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bz
		{
			set{ _bz=value;}
			get{return _bz;}
		}
		#endregion Model

	}
}

