using System;
namespace BCW.SSE.Model
{
	/// <summary>
	/// SseOrder:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SseOrder
	{
		public SseOrder()
		{}
		#region Model
		private int _id;
		private int _ordertype;
		private int _sseno;
		private DateTime _orderdatetime;
		private int _userid;
		private bool _buytype;
		private decimal _buymoney=0;
		private int _state=0;
		private string _bz;
		private bool _isautoorder= false;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int orderType
		{
			set{ _ordertype=value;}
			get{return _ordertype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int sseNo
		{
			set{ _sseno=value;}
			get{return _sseno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime orderDateTime
		{
			set{ _orderdatetime=value;}
			get{return _orderdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int userId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool buyType
		{
			set{ _buytype=value;}
			get{return _buytype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal buyMoney
		{
			set{ _buymoney=value;}
			get{return _buymoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bz
		{
			set{ _bz=value;}
			get{return _bz;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool isAutoOrder
		{
			set{ _isautoorder=value;}
			get{return _isautoorder;}
		}
		#endregion Model

	}

    //订单历史
    public partial class sseOrderHistory
    {
        public SseOrder sseOrder;           //订单信息
        public decimal backMoney;               //返奖

        public sseOrderHistory()
        {
            this.sseOrder = new SseOrder();
        }
    }
}

