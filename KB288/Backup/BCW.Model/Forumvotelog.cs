using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forumvotelog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forumvotelog
	{
		public Forumvotelog()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _adminid;
		private string _title;
		private int _bid;
		private int _forumid;
		private string _notes;
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
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 管理ID
		/// </summary>
		public int AdminId
		{
			set{ _adminid=value;}
			get{return _adminid;}
		}
		/// <summary>
		/// 帖子标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
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
		/// 论坛ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 日志内容
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

