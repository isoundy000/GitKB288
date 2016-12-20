using System;
namespace BCW.HP3.Model
{
	/// <summary>
	/// HP3BuySY:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class HP3BuySY
	{
		public HP3BuySY()
		{}
		#region Model
		private int _id;
		private int _buyid;
		private string _buydate;
		private int _buytype;
        private string _buynum;
		private long _buymoney;
        private int _buyzhu;
        private DateTime _buytime;
        private decimal _odds;
        private long _willget;
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
		public int BuyID
		{
			set{ _buyid=value;}
			get{return _buyid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BuyDate
		{
			set{ _buydate=value;}
			get{return _buydate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BuyType
		{
			set{ _buytype=value;}
			get{return _buytype;}
		}
		/// <summary>
		/// 
		/// </summary>
        public string BuyNum
        {
            set { _buynum = value; }
            get { return _buynum; }
        }
		/// <summary>
		/// 
		/// </summary>
		public long BuyMoney
		{
			set{ _buymoney=value;}
			get{return _buymoney;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int BuyZhu
        {
            set { _buyzhu = value; }
            get { return _buyzhu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BuyTime
        {
            set { _buytime = value; }
            get { return _buytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long WillGet
        {
            set { _willget = value; }
            get { return _willget; }
        }
		#endregion Model

	}
}

