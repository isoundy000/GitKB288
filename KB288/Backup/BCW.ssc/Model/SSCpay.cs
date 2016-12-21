using System;
namespace BCW.ssc.Model
{
	/// <summary>
	/// 实体类SSCpay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SSCpay
	{
		public SSCpay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _sscid;
		private int _usid;
		private string _usname;
		private long _price;
		private int _icount;
		private string _notes;
		private string _result;
		private long _prices;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
		private string _winnotes;
		private int _isspier;
        private decimal _odds;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 下注类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 期数
		/// </summary>
		public int SSCId
		{
			set{ _sscid=value;}
			get{return _sscid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 每注金额
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 多少注
		/// </summary>
		public int iCount
		{
			set{ _icount=value;}
			get{return _icount;}
		}
		/// <summary>
		/// 下注内容
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 开奖结果:1 2 3 4 5
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 下注总金额
		/// </summary>
		public long Prices
		{
			set{ _prices=value;}
			get{return _prices;}
		}
		/// <summary>
		/// 赢取金额
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态(0未开奖、1已开奖、2已兑奖)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 下注时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 赢的内容
		/// </summary>
		public string WinNotes
		{
			set{ _winnotes=value;}
			get{return _winnotes;}
		}
		/// <summary>
		/// 机器人（0非、1是）
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
        /// <summary>
        /// 赔率
        /// </summary>
        public decimal Odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
		#endregion Model

	}
}

