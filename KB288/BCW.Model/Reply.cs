using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Reply 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Reply
	{
		public Reply()
		{}
		#region Model
		private int _id;
		private int _floor;
		private int _forumid;
		private int _bid;
		private int _usid;
		private string _usname;
		private string _content;
		private int _filenum;
		private int _isgood;
		private int _istop;
		private int _isdel;
		private int _replyid;
		private string _restats;
		private DateTime _addtime;
		private string _centtext;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 楼层编号
		/// </summary>
		public int Floor
		{
			set{ _floor=value;}
			get{return _floor;}
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
		/// 帖子ID
		/// </summary>
		public int Bid
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// 回复内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 文件附件数
		/// </summary>
		public int FileNum
		{
			set{ _filenum=value;}
			get{return _filenum;}
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
		/// 是否置顶
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// 是否已删除
		/// </summary>
		public int IsDel
		{
			set{ _isdel=value;}
			get{return _isdel;}
		}
		/// <summary>
		/// 回复楼层ID
		/// </summary>
		public int ReplyId
		{
			set{ _replyid=value;}
			get{return _replyid;}
		}
		/// <summary>
		/// 详细评价
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
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
		/// 得币详情
		/// </summary>
		public string CentText
		{
			set{ _centtext=value;}
			get{return _centtext;}
		}
		#endregion Model

	}
}

