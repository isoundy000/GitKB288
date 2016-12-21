using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Ballpay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Ballpay
	{
		public Ballpay()
		{}
		#region Model
		private int _id;
		private int _ballid;
		private int _usid;
		private string _usname;
		private int _buynum;
		private int _buycount;
		private long _buycent;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 彩球ID
		/// </summary>
		public int BallId
		{
			set{ _ballid=value;}
			get{return _ballid;}
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
		/// 购买数字
		/// </summary>
		public int BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// 总购买份数
		/// </summary>
		public int BuyCount
		{
			set{ _buycount=value;}
			get{return _buycount;}
		}
		/// <summary>
		/// 总购买币数
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// 赢取币数
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态（0未开奖/1已开奖/2已兑奖）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 购买时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

