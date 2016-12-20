using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forum 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forum
	{
		public Forum()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private string _donode;
		private string _title;
		private string _notes;
		private string _logo;
		private string _content;
		private string _label;
		private int _postlt;
		private int _replylt;
		private int _gradelt;
		private int _visitlt;
		private int _showtype;
		private int _isnode;
		private int _isactive;
		private int _groupid;
		private string _visitid;
		private int _ispc;
		private int _line;
		private int _topline;
		private DateTime _toptime;
		private string _topubb;
		private string _footubb;
		private int _paixu;
		private long _icent;
		/// <summary>
		/// 论坛ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 节点ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 下级版块ID集合
		/// </summary>
		public string DoNode
		{
			set{ _donode=value;}
			get{return _donode;}
		}
		/// <summary>
		/// 版块名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 版块口号
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// LOGO图片地址
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// 版规公告
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 帖子分类标签
		/// </summary>
		public string Label
		{
			set{ _label=value;}
			get{return _label;}
		}
		/// <summary>
		/// 发贴限制（0不限制，1限版主，2管理员，3禁止发贴）
		/// </summary>
		public int Postlt
		{
			set{ _postlt=value;}
			get{return _postlt;}
		}
		/// <summary>
		/// 回复限制（同上）
		/// </summary>
		public int Replylt
		{
			set{ _replylt=value;}
			get{return _replylt;}
		}
		/// <summary>
		/// 限多少等级可进
		/// </summary>
		public int Gradelt
		{
			set{ _gradelt=value;}
			get{return _gradelt;}
		}
		/// <summary>
		/// 访问限制（0不限制，1限制会员，2VIP会员，3限制版主，4限制管理员）
		/// </summary>
		public int Visitlt
		{
			set{ _visitlt=value;}
			get{return _visitlt;}
		}
		/// <summary>
		/// 显示贴子方式  0|只显本版贴|1|显示下级帖
		/// </summary>
		public int ShowType
		{
			set{ _showtype=value;}
			get{return _showtype;}
		}
		/// <summary>
		/// 是否显示下级版块 0|不显示|1|显示
		/// </summary>
		public int IsNode
		{
			set{ _isnode=value;}
			get{return _isnode;}
		}
		/// <summary>
		/// 0正常，1暂停
		/// </summary>
		public int IsActive
		{
			set{ _isactive=value;}
			get{return _isactive;}
		}
		/// <summary>
		/// 关联圈子ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 访问限制ID（|分开）
		/// </summary>
		public string VisitId
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// 电脑访问
		/// </summary>
		public int IsPc
		{
			set {_ispc=value;}
			get {return _ispc;}
		}
		/// <summary>
		/// 当前在线
		/// </summary>
		public int Line
		{
			set{ _line=value;}
			get{return _line;}
		}
		/// <summary>
		/// 最高在线
		/// </summary>
		public int TopLine
		{
			set{ _topline=value;}
			get{return _topline;}
		}
		/// <summary>
		/// 最高在线时间
		/// </summary>
		public DateTime TopTime
		{
			set{ _toptime=value;}
			get{return _toptime;}
		}
		/// <summary>
		/// 论坛顶部UBB
		/// </summary>
		public string TopUbb
		{
			set{ _topubb=value;}
			get{return _topubb;}
		}
		/// <summary>
		/// 论坛底部UBB
		/// </summary>
		public string FootUbb
		{
			set{ _footubb=value;}
			get{return _footubb;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// 基金数目
		/// </summary>
		public long iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		#endregion Model

	}
}

