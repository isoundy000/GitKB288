using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类dawnlifegift 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class dawnlifegift
	{
		public dawnlifegift()
		{}
		#region Model
		private int _id;
		private DateTime? _date;
		private long _gift;      
		private int? _usid;
		private string _usname;
		private long _coin;
		private int? _state;
        private long _giftj;
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
		public DateTime? date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long gift
		{
			set{ _gift=value;}
			get{return _gift;}
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
		public int? state
		{
			set{ _state=value;}
			get{return _state;}
		}
        /// <summary>
        /// 
        /// </summary>
        public long giftj
        {
            set { _giftj = value; }
            get { return _giftj; }
        }
		#endregion Model

	}
}

