using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类dawnlifeDo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class dawnlifeDo
	{
		public dawnlifeDo()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private int? _sum;
		private int? _stock;
		private int? _stocky;
		private string _goods;
		private string _price;
		private string _dsg;
        private long _coin;
        //private int _n;
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
		public int? sum
		{
			set{ _sum=value;}
			get{return _sum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? stock
		{
			set{ _stock=value;}
			get{return _stock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? stocky
		{
			set{ _stocky=value;}
			get{return _stocky;}
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
		public string dsg
		{
			set{ _dsg=value;}
			get{return _dsg;}
		}
        /// <summary>
        /// 
        /// </summary>
        public long coin
        {
            set { _coin = value; }
            get { return _coin; }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int n
        //{
        //    set { _n = value; }
        //    get { return _n; }
        //}
		#endregion Model

	}
}

