using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forumvote 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forumvote
	{
		public Forumvote()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _qinum;
		private int _forumid;
		private int _bid;
		private int _usid;
		private string _usname;
		private string _notes;
		private DateTime _addtime;
		private int _iswin;
		private int _state;
		private DateTime _actime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型（1六肖/2波色/3平特一肖/4半数/5五尾/6五不中）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 六仔期数
		/// </summary>
		public int qiNum
		{
			set{ _qinum=value;}
			get{return _qinum;}
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
		/// 帖子ID
		/// </summary>
		public int BID
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
		/// 内容
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 投注时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 是否中奖(0未中/1中奖）
		/// </summary>
		public int IsWin
		{
			set{ _iswin=value;}
			get{return _iswin;}
		}
		/// <summary>
		/// 状态（0未开奖/1已开奖）
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 开奖时间
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
		}
		#endregion Model

	}
}

