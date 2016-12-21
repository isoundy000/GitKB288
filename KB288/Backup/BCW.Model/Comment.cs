using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Comment 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Comment
	{
		public Comment()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private int _types;
		private int _detailid;
		private int _userid;
		private string _username;
		private int _face;
		private string _content;
		private string _addusip;
		private DateTime _addtime;
		private string _retext;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 栏目ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 栏目类型
		/// </summary>
		public int Types
		{
			set { _types = value; }
			get { return _types; }
		}
		/// <summary>
		/// 内容ID
		/// </summary>
		public int DetailId
		{
			set{ _detailid=value;}
			get{return _detailid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 表情
		/// </summary>
		public int Face
		{
			set{ _face=value;}
			get{return _face;}
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 发表IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 发表时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回复评论内容
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		#endregion Model

	}
}

