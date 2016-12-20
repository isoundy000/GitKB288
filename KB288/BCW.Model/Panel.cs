using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Panel 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Panel
	{
		public Panel()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _purl;
		private int _usid;
		private int _isbr;
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
		/// 连接名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 连接地址
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 是否换行
		/// </summary>
		public int IsBr
		{
			set{ _isbr=value;}
			get{return _isbr;}
		}
		/// <summary>
		/// 排序编号
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		#endregion Model

	}
}

