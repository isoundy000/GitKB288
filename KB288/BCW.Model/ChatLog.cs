using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类ChatLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ChatLog
	{
		public ChatLog()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _chatid;
		private int _usid;
		private string _usname;
		private long _paycent;
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
		/// 类型（0管理日志/1捐款日志/2基金支出日志）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
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
		/// 捐款或支出金额
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
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

