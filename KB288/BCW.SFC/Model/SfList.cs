using System;
namespace BCW.SFC.Model
{
	/// <summary>
	/// 实体类SfList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SfList
	{
		public SfList()
		{}
		#region Model
		private int _id;
		private int _cid;
		private string _match;
		private string _team_home;
		private string _team_away;
		private string _start_time;
		private string _score;
		private string _result;
		private int _state;
		private long _paycent;
		private int _paycount;
		private DateTime _endtime;
		private string _other;
        private DateTime _sale_starttime;
        private long _nowprize;
        private long _nextprize;
        private long _sysprize;
        private int _sysprizestatue;
        private long _sysdayprize;
        /// <summary>
        /// 
        /// </summary>
        public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 期号
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		/// <summary>
		/// 赛事
		/// </summary>
		public string Match
		{
			set{ _match=value;}
			get{return _match;}
		}
		/// <summary>
		/// 主场球队
		/// </summary>
		public string Team_Home
		{
			set{ _team_home=value;}
			get{return _team_home;}
		}
		/// <summary>
		/// 客场球队
		/// </summary>
		public string Team_Away
		{
			set{ _team_away=value;}
			get{return _team_away;}
		}
		/// <summary>
		/// 比赛时间
		/// </summary>
		public string Start_time
		{
			set{ _start_time=value;}
			get{return _start_time;}
		}
		/// <summary>
		/// 比分
		/// </summary>
		public string Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 开奖结果
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 是否开奖（0,1）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 投注总额
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 投注数
		/// </summary>
		public int PayCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 截止时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 其他
		/// </summary>
		public string other
		{
			set{ _other=value;}
			get{return _other;}
		}
        /// <summary>
        /// 投注开始时间
        /// </summary>
        public DateTime Sale_StartTime
        {
            set { _sale_starttime = value; }
            get { return _sale_starttime; }
        }
        /// <summary>
        /// 当前奖池总额
        /// </summary>
        public long nowprize
        {
            set { _nowprize = value; }
            get { return _nowprize; }
        }
        /// <summary>
        /// 结余奖池
        /// </summary>
        public long nextprize
        {
            set { _nextprize = value; }
            get { return _nextprize; }
        }
        /// <summary>
        /// 系统投入
        /// </summary>
        public long sysprize
        {
            set { _sysprize = value; }
            get { return _sysprize; }
        }
        /// <summary>
        /// 系统投入回收否
        /// </summary>
        public int sysprizestatue
        {
            set { _sysprizestatue = value; }
            get { return _sysprizestatue; }
        }
        /// <summary>
        /// 每日系统收取手续
        /// </summary>
        public long sysdayprize
        {
            set { _sysdayprize = value; }
            get { return _sysdayprize; }
        }
        #endregion Model

    }
}

