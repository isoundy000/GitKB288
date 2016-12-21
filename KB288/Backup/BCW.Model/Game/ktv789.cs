using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类ktv789 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ktv789
	{
		public ktv789()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _stname;
		private int _paycent;
		private string _oneusname;
		private int _oneusid;
		private string _twousname;
		private int _twousid;
		private string _thrusname;
		private int _thrusid;
		private int _expir;
		private string _oneshot;
		private string _twoshot;
		private string _thrshot;
		private DateTime? _onetime;
		private DateTime? _twotime;
		private DateTime? _thrtime;
		private int _pkcount;
		private int _online;
		private int _smallpay;
		private int _bigpay;
		private int _nextshot;
		private string _lines;
        private int _goldtype;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 房间名称
		/// </summary>
		public string StName
		{
			set{ _stname=value;}
			get{return _stname;}
		}
		/// <summary>
		/// 下注币额
		/// </summary>
		public int PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 第一个用户
		/// </summary>
		public string OneUsName
		{
			set{ _oneusname=value;}
			get{return _oneusname;}
		}
		/// <summary>
		/// 第一个用户ID
		/// </summary>
		public int OneUsId
		{
			set{ _oneusid=value;}
			get{return _oneusid;}
		}
		/// <summary>
		/// 第二个用户
		/// </summary>
		public string TwoUsName
		{
			set{ _twousname=value;}
			get{return _twousname;}
		}
		/// <summary>
		/// 第二个用户ID
		/// </summary>
		public int TwoUsId
		{
			set{ _twousid=value;}
			get{return _twousid;}
		}
		/// <summary>
		/// 第三个用户
		/// </summary>
		public string ThrUsName
		{
			set{ _thrusname=value;}
			get{return _thrusname;}
		}
		/// <summary>
		/// 第三个用户ID
		/// </summary>
		public int ThrUsId
		{
			set{ _thrusid=value;}
			get{return _thrusid;}
		}
		/// <summary>
		/// 操作超时时间
		/// </summary>
		public int Expir
		{
			set{ _expir=value;}
			get{return _expir;}
		}
		/// <summary>
		/// 用户1开色结果
		/// </summary>
		public string OneShot
		{
			set{ _oneshot=value;}
			get{return _oneshot;}
		}
		/// <summary>
		/// 用户2开色结果
		/// </summary>
		public string TwoShot
		{
			set{ _twoshot=value;}
			get{return _twoshot;}
		}
		/// <summary>
		/// 用户3开色结果
		/// </summary>
		public string ThrShot
		{
			set{ _thrshot=value;}
			get{return _thrshot;}
		}
		/// <summary>
		/// 用户1操作时间
		/// </summary>
		public DateTime? OneTime
		{
			set{ _onetime=value;}
			get{return _onetime;}
		}
		/// <summary>
		/// 用户2操作时间
		/// </summary>
		public DateTime? TwoTime
		{
			set{ _twotime=value;}
			get{return _twotime;}
		}
		/// <summary>
		/// 用户3操作时间
		/// </summary>
		public DateTime? ThrTime
		{
			set{ _thrtime=value;}
			get{return _thrtime;}
		}
		/// <summary>
		/// 局数
		/// </summary>
		public int PkCount
		{
			set{ _pkcount=value;}
			get{return _pkcount;}
		}
		/// <summary>
		/// 房间在线人数
		/// </summary>
		public int Online
		{
			set{ _online=value;}
			get{return _online;}
		}
		/// <summary>
		/// 最大下注额
		/// </summary>
		public int SmallPay
		{
			set{ _smallpay=value;}
			get{return _smallpay;}
		}
		/// <summary>
		/// 最小下注额
		/// </summary>
		public int BigPay
		{
			set{ _bigpay=value;}
			get{return _bigpay;}
		}
		/// <summary>
		/// 下注桌子ID（谁有先开色的权限）
		/// </summary>
		public int NextShot
		{
			set{ _nextshot=value;}
			get{return _nextshot;}
		}
		/// <summary>
		/// 在线统计
		/// </summary>
		public string Lines
		{
			set{ _lines=value;}
			get{return _lines;}
		}
        /// <summary>
        /// 是否免币玩（0正常/1免）
        /// </summary>
        public int GoldType
        {
            set { _goldtype = value; }
            get { return _goldtype; }
        }
		#endregion Model

	}
}

