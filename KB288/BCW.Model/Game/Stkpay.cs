using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Stkpay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Stkpay
	{
		public Stkpay()
		{}
		#region Model
		private int _id;
		private int _bztype;
		private int _types;
		private int _winnum;
		private int _stkid;
		private int _usid;
		private string _usname;
		private decimal _odds;
		private long _buycent;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
        private int _isSpier;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 币种
		/// </summary>
		public int bzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
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
		/// 开奖数字
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// 上证ID
		/// </summary>
		public int StkId
		{
			set{ _stkid=value;}
			get{return _stkid;}
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
		///  赔率
		/// </summary>
		public decimal Odds
		{
			set{ _odds=value;}
			get{return _odds;}
		}
		/// <summary>
		/// 下注额
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// 赢币额
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态（0未开/1已开奖/2已兑奖）
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
        /// 状态（0会员下注/1机器人下注）
        /// </summary>
        public int isSpier
        {
            set { _isSpier = value; }
            get { return _isSpier; }
        }
		#endregion Model

	}
}

