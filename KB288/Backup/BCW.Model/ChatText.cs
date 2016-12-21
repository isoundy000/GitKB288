using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类ChatText 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ChatText
	{
		public ChatText()
		{}
		#region Model
		private int _id;
		private int _chatid;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private string _content;
		private int _iskiss;
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
		/// 聊天室ID
		/// </summary>
		public int ChatId
		{
			set{ _chatid=value;}
			get{return _chatid;}
		}
		/// <summary>
		/// 发言ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 发言昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 接收ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 接收ID昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 发言内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 是否悄悄话或动作(1/悄悄话)
		/// </summary>
		public int IsKiss
		{
			set{ _iskiss=value;}
			get{return _iskiss;}
		}
		/// <summary>
		/// 发言时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

