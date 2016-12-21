using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类dawnlifeDays 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class dawnlifeDays
	{
		public dawnlifeDays()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private int? _day;
		private string _goods;
		private string _price;
		private string _city;
		private string _area;
        private long _coin;
        private int _n;
        private string _news;
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
		public int? UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? day
		{
			set{ _day=value;}
			get{return _day;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string goods
		{
			set{ _goods=value;}
			get{return _goods;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string area
		{
			set{ _area=value;}
			get{return _area;}
		}
        /// <summary>
        /// 
        /// </summary>
        public long coin
        {
            set { _coin = value; }
            get { return _coin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int n
        {
            set { _n = value; }
            get { return _n; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string news
        {
            set { _news = value; }
            get { return _news; }
        }
		#endregion Model

	}
}

