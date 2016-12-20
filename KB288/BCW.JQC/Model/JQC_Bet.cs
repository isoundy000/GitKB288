using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// 实体类JQC_Bet 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class JQC_Bet
	{
		public JQC_Bet()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _lottery_issue;
		private string _votenum;
		private int _zhu;
		private int _zhu_money;
		private long _putgold;
		private long _getmoney;
		private int _state;
		private int _voterate;
		private DateTime _input_time;
		private int _isrobot;
        private int _a;
        private int _prize1;
        private int _prize2;
        private int _prize3;
        private int _prize4;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        public int a
        {
            set { _a = value; }
            get { return _a; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 开奖期号
		/// </summary>
		public int Lottery_issue
		{
			set{ _lottery_issue=value;}
			get{return _lottery_issue;}
		}
		/// <summary>
		/// 投注号码
		/// </summary>
		public string VoteNum
		{
			set{ _votenum=value;}
			get{return _votenum;}
		}
		/// <summary>
		/// 投注注数
		/// </summary>
		public int Zhu
		{
			set{ _zhu=value;}
			get{return _zhu;}
		}
		/// <summary>
		/// 单注金额
		/// </summary>
		public int Zhu_money
		{
			set{ _zhu_money=value;}
			get{return _zhu_money;}
		}
		/// <summary>
		/// 共投注金额
		/// </summary>
		public long PutGold
		{
			set{ _putgold=value;}
			get{return _putgold;}
		}
		/// <summary>
		/// 当期赚取的酷币
		/// </summary>
		public long GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
		}
        /// <summary>
        /// 0未开/1中/2不中/3已领/4过期    
        /// </summary>
        public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 投注倍率
		/// </summary>
		public int VoteRate
		{
			set{ _voterate=value;}
			get{return _voterate;}
		}
		/// <summary>
		/// 投注时间
		/// </summary>
		public DateTime Input_Time
		{
			set{ _input_time=value;}
			get{return _input_time;}
		}
		/// <summary>
		/// 0为会员1为机器人
		/// </summary>
		public int isRobot
		{
			set{ _isrobot=value;}
			get{return _isrobot;}
		}
        /// <summary>
        /// 中一等奖的注数
        /// </summary>
        public int Prize1
        {
            set { _prize1 = value; }
            get { return _prize1; }
        }
        /// <summary>
        /// 中二等奖的注数
        /// </summary>
        public int Prize2
        {
            set { _prize2 = value; }
            get { return _prize2; }
        }
        /// <summary>
        /// 中三等奖的注数
        /// </summary>
        public int Prize3
        {
            set { _prize3 = value; }
            get { return _prize3; }
        }
        /// <summary>
        /// 中四等奖的注数
        /// </summary>
        public int Prize4
        {
            set { _prize4 = value; }
            get { return _prize4; }
        }
        #endregion Model

    }
}

