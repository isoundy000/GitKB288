using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Chatblack 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Chatblack
	{
		public Chatblack()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _chatid;
		private string _chatname;
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
		/// 加黑类型(0禁言/1踢出)
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 被加黑用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 被加黑用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 聊天室ID
		/// </summary>
		public int ChatId
		{
			set{ _chatid=value;}
			get{return _chatid;}
		}
		/// <summary>
		/// 聊天室名称
		/// </summary>
		public string ChatName
		{
			set{ _chatname=value;}
			get{return _chatname;}
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
		/// 加黑天数或分钟数（踢出为分钟、禁言为天）
		/// </summary>
		public int BlackDay
		{
			set{ _blackday=value;}
			get{return _blackday;}
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
		/// 到期时间
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

