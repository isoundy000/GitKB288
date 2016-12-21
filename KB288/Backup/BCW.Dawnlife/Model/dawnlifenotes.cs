using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类dawnlifenotes 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class dawnlifenotes
	{
		public dawnlifenotes()
		{}
		#region Model
		private int _id;
		private int _coin;
		private int _usid;
		private int _city;
		private int _area;
		private long _money;
		private long _debt;
		private string _buy;
		private string _sell;
		private long _price;
		private long _num;
		private DateTime _date;
        private int _day;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 闯荡编号
		/// </summary>
		public int coin
		{
			set{ _coin=value;}
			get{return _coin;}
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
		/// 城市
		/// </summary>
		public int city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 地区
		/// </summary>
		public int area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// 银两
		/// </summary>
		public long money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// 欠款
		/// </summary>
		public long debt
		{
			set{ _debt=value;}
			get{return _debt;}
		}
		/// <summary>
		/// 购买物品
		/// </summary>
		public string buy
		{
			set{ _buy=value;}
			get{return _buy;}
		}
		/// <summary>
		/// 卖出物品
		/// </summary>
		public string sell
		{
			set{ _sell=value;}
			get{return _sell;}
		}
		/// <summary>
		/// 售价
		/// </summary>
		public long price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 数量
		/// </summary>
		public long num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime date
		{
			set{ _date=value;}
			get{return _date;}
		}
        /// <summary>
        /// 天数
        /// </summary>
        public int day
        {
            set { _day = value; }
            get { return _day; }
        }
		#endregion Model

	}
}

