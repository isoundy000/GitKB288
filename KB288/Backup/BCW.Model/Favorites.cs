using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Favorites 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Favorites
	{
		public Favorites()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private string _title;
		private string _purl;
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
		/// 类型/默认0/帖子类型1
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 文件夹ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
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
		/// 收藏标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 收藏地址
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// 收藏时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

