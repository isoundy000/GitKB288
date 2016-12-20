using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Group 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Group
	{
		public Group()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _city;
		private string _logo;
		private string _notes;
		private string _content;
		private int _usid;
		private long _icent;
		private int _itotal;
		private string _visitid;
		private DateTime _visittime;
		private int _iclick;
		private int _intype;
		private int _forumid;
		private int _chatid;
		private int _forumstatus;
		private int _chatstatus;
		private string _signid;
		private DateTime _signtime;
		private int _signcent;
		private string _icentpwd;
		private int _status;
		private DateTime _extime;
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
		/// 圈子类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 圈子名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 所属城市
		/// </summary>
		public string City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 圈子徽章
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// 圈子宣言
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 创建原因
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 创建ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 基金数量
		/// </summary>
		public long iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// 成员人数
		/// </summary>
		public int iTotal
		{
			set{ _itotal=value;}
			get{return _itotal;}
		}
		/// <summary>
		/// 今天访问ID
		/// </summary>
		public string VisitId
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// 访问时间
		/// </summary>
		public DateTime VisitTime
		{
			set{ _visittime=value;}
			get{return _visittime;}
		}
		/// <summary>
		/// 累积人气
		/// </summary>
		public int iClick
		{
			set{ _iclick=value;}
			get{return _iclick;}
		}
		/// <summary>
		/// 加入圈子限制（0不限制/1要验证/2不允许加入）
		/// </summary>
		public int InType
		{
			set{ _intype=value;}
			get{return _intype;}
		}
		/// <summary>
		/// 关联论坛ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 关联聊室ID
		/// </summary>
		public int ChatId
		{
			set{ _chatid=value;}
			get{return _chatid;}
		}
		/// <summary>
		/// 论坛开关（0正常/1暂停）
		/// </summary>
		public int ForumStatus
		{
			set{ _forumstatus=value;}
			get{return _forumstatus;}
		}
		/// <summary>
		/// 圈聊开关（0正常/1暂停）
		/// </summary>
		public int ChatStatus
		{
			set{ _chatstatus=value;}
			get{return _chatstatus;}
		}
		/// <summary>
		/// 签到ID
		/// </summary>
		public string SignID
		{
			set{ _signid=value;}
			get{return _signid;}
		}
		/// <summary>
		/// 签到时间
		/// </summary>
		public DateTime SignTime
		{
			set{ _signtime=value;}
			get{return _signtime;}
		}
		/// <summary>
		/// 签到得币数目
		/// </summary>
		public int SignCent
		{
			set{ _signcent=value;}
			get{return _signcent;}
		}
		/// <summary>
		/// 基金密码
		/// </summary>
		public string iCentPwd
		{
			set{ _icentpwd=value;}
			get{return _icentpwd;}
		}
		/// <summary>
		/// 圈子状态(0正常/1未开放)
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

