using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Goods 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Goods
	{
		public Goods()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private int _usid;
		private bool _isad;
		private string _title;
		private string _keyword;
		private string _files;
		private string _cover;
		private string _config;
		private string _content;
		private string _mobile;
		private decimal _citymoney;
		private decimal _vipmoney;
		private int _sellcount;
		private int _stockcount;
		private int _paytype;
		private int _posttype;
		private string _postmoney;
		private string _restats;
		private string _relastip;
		private int _readcount;
		private int _paycount;
		private int _evcount;
		private int _recount;
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
		/// 栏目分类ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 发布ID/0为系统发布
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 是否广告
		/// </summary>
		public bool IsAd
		{
			set{ _isad=value;}
			get{return _isad;}
		}
		/// <summary>
		/// 商品标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 关键字
		/// </summary>
		public string KeyWord
		{
			set{ _keyword=value;}
			get{return _keyword;}
		}
		/// <summary>
		/// 商品图片
		/// </summary>
		public string Files
		{
			set{ _files=value;}
			get{return _files;}
		}
		/// <summary>
		/// 商品封面
		/// </summary>
		public string Cover
		{
			set{ _cover=value;}
			get{return _cover;}
		}
		/// <summary>
		/// 商品配置/属性
		/// </summary>
		public string Config
		{
			set{ _config=value;}
			get{return _config;}
		}
		/// <summary>
		/// 描述内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 联系方式:QQ/TEL
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 市场价格
		/// </summary>
		public decimal CityMoney
		{
			set{ _citymoney=value;}
			get{return _citymoney;}
		}
		/// <summary>
		/// 会员价格
		/// </summary>
		public decimal VipMoney
		{
			set{ _vipmoney=value;}
			get{return _vipmoney;}
		}
		/// <summary>
		/// 已出售数量
		/// </summary>
		public int SellCount
		{
		    set { _sellcount = value; }
		    get { return _sellcount; }
		}
		/// <summary>
		/// 出售数量
		/// </summary>
		public int StockCount
		{
		    set { _stockcount = value; }
		    get { return _stockcount; }
		}
		/// <summary>
		/// 送货方式0货到付款/1当面交易/2先付款后发款
		/// </summary>
		public int PayType
		{
			set{ _paytype=value;}
			get{return _paytype;}
		}
		/// <summary>
		/// 付款币种0/人民币/1金币/2钻石
		/// </summary>
		public int PostType
		{
			set{ _posttype=value;}
			get{return _posttype;}
		}
		/// <summary>
		/// 邮递邮费(例子:15|快递|20|EMS,留空商家包邮)
		/// </summary>
		public string PostMoney
		{
			set{ _postmoney=value;}
			get{return _postmoney;}
		}
		/// <summary>
		/// 评价详细
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// 最后回复IP
		/// </summary>
		public string ReLastIP
		{
		    set { _relastip = value; }
		    get { return _relastip; }
		}
		/// <summary>
		/// 点击量
		/// </summary>
		public int Readcount
		{
			set{ _readcount=value;}
			get{return _readcount;}
		}
		/// <summary>
		/// 评论数
		/// </summary>
		public int Recount
		{
			set{ _recount=value;}
			get{return _recount;}
		}
		/// <summary>
		/// 购买人数
		/// </summary>
		public int Paycount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 评价人数
		/// </summary>
		public int Evcount
		{
			set{ _evcount=value;}
			get{return _evcount;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

