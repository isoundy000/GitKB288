using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// 实体类JQC_Internet 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class JQC_Internet
	{
		public JQC_Internet()
		{}
		#region Model
		private int _id;
		private int _phase;
		private string _match;
		private string _team_home;
		private string _team_away;
		private DateTime _sale_start;
		private DateTime _sale_end;
		private string _start_time;
		private string _score;
		private string _result;
        private int _nowprize;
        private string _zhu;
        private string _zhu_money;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 期号
		/// </summary>
		public int phase
		{
			set{ _phase=value;}
			get{return _phase;}
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
		/// 销售开始时间
		/// </summary>
		public DateTime Sale_Start
		{
			set{ _sale_start=value;}
			get{return _sale_start;}
		}
		/// <summary>
		/// 销售截止时间
		/// </summary>
		public DateTime Sale_End
		{
			set{ _sale_end=value;}
			get{return _sale_end;}
		}
		/// <summary>
		/// 比赛开始时间
		/// </summary>
		public string Start_Time
		{
			set{ _start_time=value;}
			get{return _start_time;}
		}
		/// <summary>
		/// 比赛比分
		/// </summary>
		public string Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 比赛结果
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
        /// <summary>
        /// 当前期号的奖池
        /// </summary>
        public int nowprize
        {
            set { _nowprize = value; }
            get { return _nowprize; }
        }
        /// <summary>
        /// 该期投注注数
        /// </summary>
        public string zhu
        {
            set { _zhu = value; }
            get { return _zhu; }
        }
        /// <summary>
        /// 每注金额
        /// </summary>
        public string zhu_money
        {
            set { _zhu_money = value; }
            get { return _zhu_money; }
        }
        #endregion Model

    }
}

