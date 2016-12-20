using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Statinfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Statinfo
	{
		public Statinfo()
		{}
		#region Model
		private int _id;
		private string _ip;
		private string _purl;
		private string _browser;
		private string _system;
		private DateTime _addtime;
        private int _ipcount;
        private int _browsercount;
        private int _systemcount;
        private int _purlcount;
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
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Browser
		{
			set{ _browser=value;}
			get{return _browser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string System
		{
			set{ _system=value;}
			get{return _system;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int IpCount
        {
            set { _ipcount = value; }
            get { return _ipcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BrowserCount
        {
            set { _browsercount = value; }
            get { return _browsercount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SystemCount
        {
            set { _systemcount = value; }
            get { return _systemcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PUrlCount
        {
            set { _purlcount = value; }
            get { return _purlcount; }
        }
		#endregion Model

	}
}

