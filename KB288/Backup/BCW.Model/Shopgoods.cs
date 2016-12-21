using System;
namespace BCW.Shop.Model
{
	/// <summary>
	/// 实体类Shopgoods 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Shopgoods
	{
		public Shopgoods()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _giftid;
		private string _prevpic;
		private int _num;
		private DateTime _buytime;
		private DateTime? _sendtime;
		private DateTime? _receivetime;
		private int _usid;
		private string _address;
		private string _phone;
		private string _email;
		private string _realname;
		private string _message;
        private int _ShopGiftId;
        private string _Express;
        private string _Expressnum;

		/// <summary>
		/// 标识ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 物品名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 物品Id
		/// </summary>
		public int GiftId
		{
			set{ _giftid=value;}
			get{return _giftid;}
		}
		/// <summary>
		/// 图片
		/// </summary>
		public string PrevPic
		{
			set{ _prevpic=value;}
			get{return _prevpic;}
		}
		/// <summary>
		/// 物品数量
		/// </summary>
		public int Num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// 购买时间
		/// </summary>
		public DateTime BuyTime
		{
			set{ _buytime=value;}
			get{return _buytime;}
		}
		/// <summary>
		/// 发货时间
		/// </summary>
		public DateTime? SendTime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// 收货时间
		/// </summary>
		public DateTime? ReceiveTime
		{
			set{ _receivetime=value;}
			get{return _receivetime;}
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
		/// 收货地址
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 联系方式
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 姓名
		/// </summary>
		public string RealName
		{
			set{ _realname=value;}
			get{return _realname;}
		}
		/// <summary>
		/// 留言
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
            	/// <summary>
		/// 奖品id
		/// </summary>
        public int ShopGiftId
		{
            set { _ShopGiftId = value; }
            get { return _ShopGiftId; }
		}
        /// <summary>
        /// 快递公司
        /// </summary>
        public string Express
        {
            set { _Express = value; }
            get { return _Express; }
        }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string Expressnum
        {
            set { _Expressnum = value; }
            get { return _Expressnum; }
        }
		#endregion Model

	}
}

