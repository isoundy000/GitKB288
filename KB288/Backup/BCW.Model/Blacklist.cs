using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Blacklist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Blacklist
	{
		public Blacklist()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _forumid;
		private string _forumname;
		private string _blackrole;
		private string _blackwhy;
		private int _blackday;
		private int _include;
		private int _adminusid;
		private DateTime _exittime;
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
		/// 论坛ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 论坛名称
		/// </summary>
		public string ForumName
		{
			set{ _forumname=value;}
			get{return _forumname;}
		}
		/// <summary>
		/// 加黑的权限
		/// </summary>
		public string BlackRole
		{
			set{ _blackrole=value;}
			get{return _blackrole;}
		}
		/// <summary>
		/// 加黑理由
		/// </summary>
		public string BlackWhy
		{
			set{ _blackwhy=value;}
			get{return _blackwhy;}
		}
		/// <summary>
		/// 加黑的天数
		/// </summary>
		public int BlackDay
		{
			set{ _blackday=value;}
			get{return _blackday;}
		}
		/// <summary>
		/// 加黑是否包含下级版块
		/// </summary>
		public int Include
		{
			set{ _include=value;}
			get{return _include;}
		}
		/// <summary>
		/// 操作ID
		/// </summary>
		public int AdminUsID
		{
			set{ _adminusid=value;}
			get{return _adminusid;}
		}
		/// <summary>
		/// 自动解黑时间
		/// </summary>
		public DateTime ExitTime
		{
			set{ _exittime=value;}
			get{return _exittime;}
		}
		/// <summary>
		/// 加黑时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

