using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forumlog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forumlog
	{
		public Forumlog()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _forumid;
		private int _bid;
		private int _reid;
		private string _content;
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
		/// 日志类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
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
		/// 帖子ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
		}
		/// <summary>
		/// 回复ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// 日志内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

