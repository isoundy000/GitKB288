using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Role 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Role
	{
		public Role()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private string _rolece;
		private string _rolename;
		private int _forumid;
		private string _forumname;
		private DateTime _starttime;
		private DateTime _overtime;
		private int _include;
		private int _status;
		private string _addname;
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
		/// 用户权限
		/// </summary>
		public string Rolece
		{
			set{ _rolece=value;}
			get{return _rolece;}
		}
		/// <summary>
		/// 权限职称
		/// </summary>
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// 管理的某论坛ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 管理的某论坛名称
		/// </summary>
		public string ForumName
		{
			set{ _forumname=value;}
			get{return _forumname;}
		}
		/// <summary>
		/// 上任时间
		/// </summary>
		public DateTime StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 离任时间
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 是否包含下级版块
		/// </summary>
		public int Include
		{
			set{ _include=value;}
			get{return _include;}
		}
		/// <summary>
		/// 管理状态（0正任/1已停任/2荣誉版）
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 版主宣言
		/// </summary>
		public string AddName
		{
			set{ _addname=value;}
			get{return _addname;}
		}
		#endregion Model

	}
}

