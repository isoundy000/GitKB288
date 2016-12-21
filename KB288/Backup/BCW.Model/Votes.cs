using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Votes 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Votes
	{
		public Votes()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _title;
		private string _content;
		private string _vote;
		private string _addvote;
		private string _voteid;
		private int _votetype;
		private int _voteleven;
		private int _votetiple;
		private string _restats;
		private int _readcount;
		private int _status;
		private DateTime _voteextime;
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
		/// 类型（0为系统发布/非0为会员贴子内发布）
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
		/// 投票标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 内容描述
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 投票选项（用#分开）
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// 对应投票数
		/// </summary>
		public string AddVote
		{
			set{ _addvote=value;}
			get{return _addvote;}
		}
		/// <summary>
		/// 投票ID
		/// </summary>
		public string VoteID
		{
			set{ _voteid=value;}
			get{return _voteid;}
		}
		/// <summary>
		/// 投票类型
		/// </summary>
		public int VoteType
		{
			set{ _votetype=value;}
			get{return _votetype;}
		}
		/// <summary>
		/// 投票等级
		/// </summary>
		public int VoteLeven
		{
			set{ _voteleven=value;}
			get{return _voteleven;}
		}
		/// <summary>
		/// 投票性质
		/// </summary>
		public int VoteTiple
		{
			set{ _votetiple=value;}
			get{return _votetiple;}
		}
		/// <summary>
		/// 评价
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// 人气
		/// </summary>
		public int Readcount
		{
			set{ _readcount=value;}
			get{return _readcount;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int Status
		{
		    set { _status=value;}
		    get { return _status;}
		}
		/// <summary>
		/// 截止投票时间
		/// </summary>
		public DateTime VoteExTime
		{
			set{ _voteextime=value;}
			get{return _voteextime;}
		}
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

