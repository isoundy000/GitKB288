using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Luckpay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Luckpay
	{
		public Luckpay()
		{}
		#region Model
		private int _id;
		private int _luckid;
		private int _usid;
		private string _usname;
		private string _buynum;
		private long _buycent;
        private long _buycents;
		private long _wincent;
		private int _state;
        private string _buytype;
		private DateTime _addtime;
        private int _isrobot;
        private string _odds;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
		/// 下注赔率
		/// </summary>
		public string odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
        /// <summary>
        /// 是否机械人下注 0不是， 1是
        /// </summary>
        public int IsRobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
		/// <summary>
		/// 幸运记录ID
		/// </summary>
		public int LuckId
		{
			set{ _luckid=value;}
			get{return _luckid;}
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
        /// 购买的类型
        /// </summary>
        public string BuyType
        {
            set { _buytype = value; }
            get { return _buytype; }
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
		/// 购买的幸运数字（逗号分开）
		/// </summary>
		public string BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// 下注币数
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
        /// <summary>
        /// 下注总币数
        /// </summary>
        public long BuyCents
        {
            set { _buycents = value; }
            get { return _buycents; }
        }
		/// <summary>
		/// 赢币数
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态（0未完成/1已完成/2已兑奖）
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
		#endregion Model

	}
}

