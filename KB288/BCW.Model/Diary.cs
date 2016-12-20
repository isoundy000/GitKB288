using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Diary 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Diary
	{
		public Diary()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private string _title;
		private string _weather;
		private string _content;
		private int _usid;
		private string _usname;
		private int _istop;
		private int _isgood;
		private int _replynum;
		private int _readnum;
		private string _addusip;
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
		/// 日记分类ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 日记标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 天气
		/// </summary>
		public string Weather
		{
			set{ _weather=value;}
			get{return _weather;}
		}
		/// <summary>
		/// 日记内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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
		/// 是否置顶
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// 是否精华
		/// </summary>
		public int IsGood
		{
			set{ _isgood=value;}
			get{return _isgood;}
		}
		/// <summary>
		/// 评论数
		/// </summary>
		public int ReplyNum
		{
			set{ _replynum=value;}
			get{return _replynum;}
		}
		/// <summary>
		/// 阅读数
		/// </summary>
		public int ReadNum
		{
			set{ _readnum=value;}
			get{return _readnum;}
		}
		/// <summary>
		/// 提交IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

