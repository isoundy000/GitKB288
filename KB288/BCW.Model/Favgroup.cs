using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Favgroup 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Favgroup
	{
		public Favgroup()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _usid;
		private int _paixu;
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
		/// 类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 收藏夹名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
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
		/// 排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
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

