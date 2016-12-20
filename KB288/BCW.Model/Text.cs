using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Text 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Text
	{
		public Text()
		{}
		#region Model
		private int _id;
		private int _forumid;
		private int _types;
		private int _labelid;
		private string _title;
		private string _content;
		private string _hidecontent;
		private int _usid;
		private string _usname;
		private int _replynum;
		private string _replyid;
		private int _readnum;
		private int _isgood;
		private int _isrecom;
		private int _islock;
		private int _istop;
		private long _prices;
		private int _price;
		private int _price2;
		private long _pricel;
		private int _bztype;
		private int _hidetype;
		private string _payid;
		private string _payci;
		private int _isseen;
		private int _isover;
		private int _isdel;
		private string _restats;
		private string _relist;
		private int _filenum;
		private int _tsid;
		private int _isflow;
		private DateTime _addtime;
		private DateTime _retime;
		private DateTime _flowtime;
        private DateTime _praisetime;
		private int _gaddnum;
		private int _gwinnum;
		private int _glznum;
        private int _gmnum;
        private int _gqinum;
        private int _praise;
        private string __praiseid;
        private string _PricesLimit;
        private string _IsPriceID;
        private int _istxt;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 派币附言，回复着附言才派币
        /// </summary>
        public string PricesLimit
        {
            set { _PricesLimit = value; }
            get { return _PricesLimit; }
        }
        /// <summary>
        /// 点赞数 默认为0
        /// </summary>
        public int Praise
        {
            set { _praise = value; }
            get { return _praise; }
        }
        /// <summary>
        /// 已派币ID
        /// </summary>
        public string IsPriceID
        {
            set { _IsPriceID = value; }
            get { return _IsPriceID; }
        }
        /// <summary>
        /// 点赞人的ID
        /// </summary>
        public string PraiseID
        {
            set { __praiseid = value; }
            get { return __praiseid; }
        }
        /// <summary>
        /// 更新点赞时间
        /// </summary>
        public DateTime PraiseTime
        {
            set { _praisetime = value; }
            get { return _praisetime; }
        }
		/// <summary>
		/// 所属论坛ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 帖子类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 帖子标签类型
		/// </summary>
		public int LabelId
		{
			set{ _labelid=value;}
			get{return _labelid;}
		}
		/// <summary>
		/// 帖子标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 帖子内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 隐藏内容
		/// </summary>
		public string HideContent
		{
			set{ _hidecontent=value;}
			get{return _hidecontent;}
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
		/// 回复数
		/// </summary>
		public int ReplyNum
		{
			set{ _replynum=value;}
			get{return _replynum;}
		}
		/// <summary>
		/// 回复ID
		/// </summary>
		public string ReplyID
		{
		    set {_replyid=value;}
		    get {return _replyid;}
		}
		/// <summary>
		/// 点击数
		/// </summary>
		public int ReadNum
		{
			set{ _readnum=value;}
			get{return _readnum;}
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
		/// 是否推荐
		/// </summary>
		public int IsRecom
		{
			set{ _isrecom=value;}
			get{return _isrecom;}
		}
		/// <summary>
		/// 是否已锁定
		/// </summary>
		public int IsLock
		{
			set{ _islock=value;}
			get{return _islock;}
		}
		/// <summary>
		/// 是否置顶或者固底
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// 派币总额
		/// </summary>
		public long Prices
		{
		    set {_prices=value;}
		    get {return _prices;}
		}
		/// <summary>
		/// 收费或每人最小派币数或等级可见的等级 
		/// </summary>
		public int Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 每人最大派币数
		/// </summary>
		public int Price2
		{
			set{ _price2=value;}
			get{return _price2;}
		}
		/// <summary>
		/// 已派币多少
		/// </summary>
		public long Pricel
		{
		    set {_pricel=value;}
		    get {return _pricel;}
		}
		/// <summary>
		/// 派币币种
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 隐藏内容性质（0默认，1回复可见，2付费可见）
		/// </summary>
		public int HideType
		{
			set{ _hidetype=value;}
			get{return _hidetype;}
		}
		/// <summary>
		/// 购买收费帖ID
		/// </summary>
		public string PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// 派币楼层尾数（多个用#分开，0为不限）
		/// </summary>
		public string PayCi
		{
			set{ _payci=value;}
			get{return _payci;}
		}
		/// <summary>
		/// 0正常/1登录可见/2手机可见/3等级可见
		/// </summary>
		public int IsSeen
		{
			set{ _isseen=value;}
			get{return _isseen;}
		}
		/// <summary>
		/// 是否已结贴
		/// </summary>
		public int IsOver
		{
			set{ _isover=value;}
			get{return _isover;}
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
		/// 详细评价
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// 评价ID列表
		/// </summary>
		public string ReList
		{
			set{ _relist=value;}
			get{return _relist;}
		}
		/// <summary>
		/// 帖子文件数
		/// </summary>
		public int FileNum
		{
			set{ _filenum=value;}
			get{return _filenum;}
		}
		/// <summary>
		/// 专题标识ID
		/// </summary>
		public int TsID
		{
			set{ _tsid=value;}
			get{return _tsid;}
		}
		/// <summary>
		/// 是否滚动（1滚动/2全区滚动）
		/// </summary>
		public int IsFlow
		{
			set{ _isflow=value;}
			get{return _isflow;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回复时间
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// 滚动截止时间
		/// </summary>
		public DateTime FlowTime
		{
			set{ _flowtime=value;}
			get{return _flowtime;}
		}
		/// <summary>
		/// 高手坛:参与次数
		/// </summary>
		public int Gaddnum
		{
			set{ _gaddnum=value;}
			get{return _gaddnum;}
		}
		/// <summary>
		/// 高手坛:中奖次数
		/// </summary>
		public int Gwinnum
		{
			set{ _gwinnum=value;}
			get{return _gwinnum;}
		}
		/// <summary>
		/// 高手坛:连中次数
		/// </summary>
		public int Glznum
		{
			set{ _glznum=value;}
			get{return _glznum;}
		}
        /// <summary>
        /// 高手坛:月中次数
        /// </summary>
        public int Gmnum
        {
            set { _gmnum = value; }
            get { return _gmnum; }
        }
        /// <summary>
        /// 高手坛:本期期数
        /// </summary>
        public int Gqinum
        {
            set { _gqinum = value; }
            get { return _gqinum; }
        }
        /// <summary>
        /// 是否已采集TXT
        /// </summary>
        public int Istxt
        {
            set { _istxt = value; }
            get { return _istxt; }
        }
		#endregion Model

	}
}

