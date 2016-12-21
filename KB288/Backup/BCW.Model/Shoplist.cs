using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Shoplist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Shoplist
	{
		public Shoplist()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _paycount;
		private int _paixu;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型（0礼物/1道具）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 分类名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 总购买数
		/// </summary>
		public int PayCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		#endregion Model

	}
}

