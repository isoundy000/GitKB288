using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Marry 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Marry
	{
		public Marry()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _ussex;
		private int _reid;
		private string _rename;
		private int _resex;
		private string _oath;
		private string _oath2;
		private int _isparty;
		private DateTime _addtime;
		private int _acusid;
		private DateTime _actime;
		private int _state;
        private string _lovestat;
		private string _homename;
		private int _flownum;
		private int _homeclick;
        private string _flowstat;
        private string _flowtimes;
        private string _marrypk;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 状态（0恋爱中/1结婚/2离婚）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 结婚人ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 结婚人昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 结婚人性别
		/// </summary>
		public int UsSex
		{
			set{ _ussex=value;}
			get{return _ussex;}
		}
		/// <summary>
		/// 结婚人ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// 结婚人昵称
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// 结婚人性别
		/// </summary>
		public int ReSex
		{
			set{ _resex=value;}
			get{return _resex;}
		}
		/// <summary>
		/// 结婚男誓言
		/// </summary>
		public string Oath
		{
			set{ _oath=value;}
			get{return _oath;}
		}
		/// <summary>
		/// 离婚原因(结婚女誓言)
		/// </summary>
		public string Oath2
		{
			set{ _oath2=value;}
			get{return _oath2;}
		}
		/// <summary>
		/// 是否已摆宴席
		/// </summary>
		public int IsParty
		{
			set{ _isparty=value;}
			get{return _isparty;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 操作ID
		/// </summary>
		public int AcUsID
		{
			set{ _acusid=value;}
			get{return _acusid;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
		}
		/// <summary>
		/// 接受请求的状态
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
        /// <summary>
        /// 恋爱红心统计
        /// </summary>
        public string LoveStat
        {
            set { _lovestat = value; }
            get { return _lovestat; }
        }
		/// <summary>
		/// 花园名称
		/// </summary>
		public string HomeName
		{
			set{ _homename=value;}
			get{return _homename;}
		}
		/// <summary>
		/// 玫瑰数量
		/// </summary>
		public int FlowNum
		{
			set{ _flownum=value;}
			get{return _flownum;}
		}
		/// <summary>
		/// 花园人气
		/// </summary>
		public int HomeClick
		{
			set{ _homeclick=value;}
			get{return _homeclick;}
		}
        /// <summary>
        /// 鲜花统计
        /// </summary>
        public string FlowStat
        {
            set { _flowstat = value; }
            get { return _flowstat; }
        }
        /// <summary>
        /// 浇花时间（包括男女双方的时间存储）
        /// </summary>
        public string FlowTimes
        {
            set { _flowtimes = value; }
            get { return _flowtimes; }
        }
        /// <summary>
        /// 结婚证地址
        /// </summary>
        public string MarryPk
        {
            set { _marrypk = value; }
            get { return _marrypk; }
        }
		#endregion Model

	}
}

