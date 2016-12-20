using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类DrawTen 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DrawTen
	{
		public DrawTen()
		{}
		#region Model
		private int _id;
		private int? _goodscounts;
		private int? _rank;
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
		public int? GoodsCounts
		{
			set{ _goodscounts=value;}
			get{return _goodscounts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Rank
		{
			set{ _rank=value;}
			get{return _rank;}
		}
		#endregion Model

	}
}

