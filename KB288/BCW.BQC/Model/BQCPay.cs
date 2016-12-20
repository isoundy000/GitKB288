using System;
namespace BCW.BQC.Model
{
	/// <summary>
	/// 实体类BQCPay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BQCPay
	{
		public BQCPay()
		{}
		#region Model
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
        private int _isprize2;
        private int _prize2num;
        private int? _isspier;
		private int _id;
		private int? _change;
		/// <summary>
		/// 
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
		/// 
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? VoteNum
		{
			set{ _votenum=value;}
			get{return _votenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OverRide
		{
			set{ _override=value;}
			get{return _override;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long PayCents
		{
			set{ _paycents=value;}
			get{return _paycents;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsPrize
		{
			set{ _isprize=value;}
			get{return _isprize;}
		}

        public int IsPrize2
        {
            set { _isprize2 = value; }
            get { return _isprize2; }
        }

        public int Prize2Num
        {
            set { _prize2num = value; }
            get { return _prize2num; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? change
		{
			set{ _change=value;}
			get{return _change;}
		}
		#endregion Model

	}
}

