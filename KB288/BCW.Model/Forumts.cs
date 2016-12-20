using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forumts 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forumts
	{
		public Forumts()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _forumid;
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
		/// 专题标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 论坛ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 论坛排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		#endregion Model

	}
}

