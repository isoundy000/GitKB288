using System;
namespace BCW.XinKuai3.Model
{
	/// <summary>
	/// 实体类XK3_Internet_Data 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class XK3_Internet_Data
	{
		public XK3_Internet_Data()
		{}
		#region Model
		private int _id;
		private string _lottery_issue;
		private string _lottery_num;
		private DateTime _lottery_time;
		private DateTime _updatetime;
		private string _sum;
		private string _three_same_all;
		private string _three_same_single;
		private string _three_same_not;
		private string _three_continue_all;
		private string _two_same_all;
		private string _two_same_single;
		private string _two_dissame;
		private string _daxiao;
		private string _danshuang;
        private int _aa;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        //=========================================
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }

        //=========================================
		/// <summary>
		/// 开奖期号
		/// </summary>
		public string Lottery_issue
		{
			set{ _lottery_issue=value;}
			get{return _lottery_issue;}
		}
		/// <summary>
		/// 开奖号码
		/// </summary>
		public string Lottery_num
		{
			set{ _lottery_num=value;}
			get{return _lottery_num;}
		}
		/// <summary>
		/// 开奖时间
		/// </summary>
		public DateTime Lottery_time
		{
			set{ _lottery_time=value;}
			get{return _lottery_time;}
		}
		/// <summary>
		/// 开奖更新时间
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 和值
		/// </summary>
		public string Sum
		{
			set{ _sum=value;}
			get{return _sum;}
		}
		/// <summary>
		/// 三同号通选
		/// </summary>
		public string Three_Same_All
		{
			set{ _three_same_all=value;}
			get{return _three_same_all;}
		}
		/// <summary>
		/// 三同号单选
		/// </summary>
		public string Three_Same_Single
		{
			set{ _three_same_single=value;}
			get{return _three_same_single;}
		}
		/// <summary>
		/// 三不同号
		/// </summary>
		public string Three_Same_Not
		{
			set{ _three_same_not=value;}
			get{return _three_same_not;}
		}
		/// <summary>
		/// 三连号通选
		/// </summary>
		public string Three_Continue_All
		{
			set{ _three_continue_all=value;}
			get{return _three_continue_all;}
		}
		/// <summary>
		/// 二同号复选
		/// </summary>
		public string Two_Same_All
		{
			set{ _two_same_all=value;}
			get{return _two_same_all;}
		}
		/// <summary>
		/// 二同号单选
		/// </summary>
		public string Two_Same_Single
		{
			set{ _two_same_single=value;}
			get{return _two_same_single;}
		}
		/// <summary>
		/// 二不同号
		/// </summary>
		public string Two_dissame
		{
			set{ _two_dissame=value;}
			get{return _two_dissame;}
		}
		/// <summary>
		/// 大小
		/// </summary>
		public string DaXiao
		{
			set{ _daxiao=value;}
			get{return _daxiao;}
		}
		/// <summary>
		/// 单双
		/// </summary>
		public string DanShuang
		{
			set{ _danshuang=value;}
			get{return _danshuang;}
		}
		#endregion Model

	}
}

