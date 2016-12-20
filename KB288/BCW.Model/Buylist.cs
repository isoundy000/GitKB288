using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Buylist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Buylist
	{
		public Buylist()
		{}
		#region Model
		private int _id;
		private string _tingno;
		private int _nodeid;
		private int _goodsid;
		private string _title;
		private decimal _price;
		private int _paycount;
		private int _postmoney;
		private int _sellid;
		private int _userid;
		private string _username;
		private string _realname;
		private string _mobile;
		private string _address;
		private string _notes;
        private string _lanotes;
		private decimal _acprice;
		private int _acstats;
        private string _acems;
		private string _actext;
		private string _addusip;
		private DateTime _addtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TingNo
		{
			set{ _tingno=value;}
			get{return _tingno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsId
		{
			set{ _goodsid=value;}
			get{return _goodsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Paycount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PostMoney
		{
			set{ _postmoney=value;}
			get{return _postmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SellId
		{
			set{ _sellid=value;}
			get{return _sellid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RealName
		{
			set{ _realname=value;}
			get{return _realname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string LaNotes
        {
            set { _lanotes = value; }
            get { return _lanotes; }
        }
		/// <summary>
		/// 
		/// </summary>
		public decimal AcPrice
		{
			set{ _acprice=value;}
			get{return _acprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AcStats
		{
			set{ _acstats=value;}
			get{return _acstats;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string AcEms
        {
            set { _acems = value; }
            get { return _acems; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string AcText
		{
			set{ _actext=value;}
			get{return _actext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

