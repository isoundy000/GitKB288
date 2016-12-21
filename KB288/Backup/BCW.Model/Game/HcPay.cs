using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类HcPay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class HcPay
	{
		public HcPay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _cid;
		private int _usid;
		private string _usname;
		private string _vote;
		private long _paycent;
        private long _paycents;
		private string _result;
		private int _state;
		private long _wincent;
		private int _bztype;
		private DateTime _addtime;
		private int _isspier;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
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
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
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
		/// 昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 投注内容
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// 下注额
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
        /// <summary>
        /// 下注总额
        /// </summary>
        public long PayCents
        {
            set { _paycents = value; }
            get { return _paycents; }
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
		/// 状态（0未开奖\1已开奖）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
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
		/// 币种 
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
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
		/// 是否机器人
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
		#endregion Model

	}
}

