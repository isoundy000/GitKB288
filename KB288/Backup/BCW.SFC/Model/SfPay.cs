using System;
namespace BCW.SFC.Model
{
	/// <summary>
	/// 实体类SfPay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SfPay
	{
		public SfPay()
		{}
		#region Model
		private int _id;
		private int _cid;
		private int? _usid;
		private string _vote;
		private int? _votenum;
		private int? _override;
		private int? _paycent;
		private long _paycents;
		private int? _state;
		private long _wincent;
		private DateTime? _addtime;
		private int? _isprize;
		private int _isspier;
        private string _change;
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
		/// 
		/// </summary>
		public int? usID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 投注
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// 投注数
		/// </summary>
		public int? VoteNum
		{
			set{ _votenum=value;}
			get{return _votenum;}
		}
		/// <summary>
		/// 投注倍率
		/// </summary>
		public int? OverRide
		{
			set{ _override=value;}
			get{return _override;}
		}
		/// <summary>
		/// 单注金额
		/// </summary>
		public int? PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 总下注额
		/// </summary>
		public long PayCents
		{
			set{ _paycents=value;}
			get{return _paycents;}
		}
		/// <summary>
		/// 兑奖（0,1）
		/// </summary>
		public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 中奖金额
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 中奖（0未中奖，1中二等，2中一等）
		/// </summary>
		public int? IsPrize
		{
			set{ _isprize=value;}
			get{return _isprize;}
		}
		/// <summary>
		/// 机器人判断（0不是，1是）
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
        /// <summary>
		/// 防止多插入数据字段
		/// </summary>
		public string change
        {
            set { _change = value; }
            get { return _change; }
        }
        #endregion Model

    }
}

