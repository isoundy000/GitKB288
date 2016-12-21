using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类dawnlifeUser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class dawnlifeUser
	{
		public dawnlifeUser()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private long _coin;
		private long _money;
		private long _debt;
		private int? _health;
		private int? _reputation;
		private string _storehouse;
		private string _stock;
        private string _city;
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
		public long coin
		{
			set{ _coin=value;}
			get{return _coin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long debt
		{
			set{ _debt=value;}
			get{return _debt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? health
		{
			set{ _health=value;}
			get{return _health;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? reputation
		{
			set{ _reputation=value;}
			get{return _reputation;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string storehouse
		{
			set{ _storehouse=value;}
			get{return _storehouse;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string stock
		{
			set{ _stock=value;}
			get{return _stock;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string city
        {
            set { _city = value; }
            get { return _city; }
        }
		#endregion Model

	}
}

