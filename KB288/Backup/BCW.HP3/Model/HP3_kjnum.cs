using System;
namespace BCW.HP3.Model
{
	/// <summary>
	/// HP3_kjnum:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class HP3_kjnum
	{
		public HP3_kjnum()
		{}
		#region Model
		private string _datenum;
        private DateTime _datetime;
		private string _fnum;
		private string _snum;
		private string _tnum;
        private string _winum;
		/// <summary>
		/// 
		/// </summary>
		public string datenum
		{
			set{ _datenum=value;}
			get{return _datenum;}
		}
        /// <summary>
        /// 
        /// </summary>
        public DateTime datetime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string Fnum
		{
			set{ _fnum=value;}
			get{return _fnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Snum
		{
			set{ _snum=value;}
			get{return _snum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tnum
		{
			set{ _tnum=value;}
			get{return _tnum;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string Winum
        {
            set { _winum = value; }
            get { return _winum; }
        }
		#endregion Model

	}
}

