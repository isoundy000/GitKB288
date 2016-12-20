using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类HcList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class HcList
	{
		public HcList()
		{}
		#region Model
		private int _id;
		private int _cid;
		private string _result;
		private string _notes;
		private int _state;
        private long _paycent;
        private int _paycount;
        private long _paycent2;
        private int _paycount2;
		private DateTime _endtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 期数
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
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
		/// 保留
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 状态（0未开奖/1已开奖）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
        /// <summary>
        /// 下注总额
        /// </summary>
        public long payCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// 下注总注额
        /// </summary>
        public int payCount
        {
            set { _paycount = value; }
            get { return _paycount; }
        }
        /// <summary>
        /// 下注总额2
        /// </summary>
        public long payCent2
        {
            set { _paycent2 = value; }
            get { return _paycent2; }
        }
        /// <summary>
        /// 下注总注额2
        /// </summary>
        public int payCount2
        {
            set { _paycount2 = value; }
            get { return _paycount2; }
        }
		/// <summary>
		/// 截止下注时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

