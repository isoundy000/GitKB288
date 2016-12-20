using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Spkblack 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Spkblack
	{
		public Spkblack()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private string _blackwhy;
		private int _blackday;
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
		/// 游戏ID标识
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
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
		/// 加黑原因
		/// </summary>
		public string BlackWhy
		{
			set{ _blackwhy=value;}
			get{return _blackwhy;}
		}
		/// <summary>
		/// 加黑天数
		/// </summary>
		public int BlackDay
		{
			set{ _blackday=value;}
			get{return _blackday;}
		}
		/// <summary>
		/// 管理ID
		/// </summary>
		public int AdminUsID
		{
			set{ _adminusid=value;}
			get{return _adminusid;}
		}
		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime ExitTime
		{
			set{ _exittime=value;}
			get{return _exittime;}
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

