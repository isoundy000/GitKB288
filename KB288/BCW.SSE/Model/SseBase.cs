using System;
namespace BCW.SSE.Model
{
	/// <summary>
	/// SseBase:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SseBase
	{
		public SseBase()
		{}
		#region Model
		private int _id;
		private int _sseno;
		private decimal _closeprice;
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
		public int sseNo
		{
			set{ _sseno=value;}
			get{return _sseno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal closePrice
		{
			set{ _closeprice=value;}
			get{return _closeprice;}
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

