using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类flowzz 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class flowzz
	{
		public flowzz()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _cnum;
		private int _price;
		private int _leven;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 种子名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 可产多少朵花
		/// </summary>
		public int cNum
		{
			set{ _cnum=value;}
			get{return _cnum;}
		}
		/// <summary>
		/// 单价
		/// </summary>
		public int Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 需要多少等级才可以购买
		/// </summary>
		public int Leven
		{
			set{ _leven=value;}
			get{return _leven;}
		}
		#endregion Model

	}
}

