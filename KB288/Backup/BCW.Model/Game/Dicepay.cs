using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Dicepay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Dicepay
	{
		public Dicepay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _diceid;
		private int _usid;
		private string _usname;
		private int _buynum;
		private int _buycount;
		private long _buycent;
		private long _wincent;
		private int _state;
		private int _bztype;
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
		/// 押注类型 1押大小/2押单双/3押总和/4押对子/5押豹子 
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 期数ID
		/// </summary>
		public int DiceId
		{
			set{ _diceid=value;}
			get{return _diceid;}
		}
		/// <summary>
		/// 会员ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 会员昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 押注选项 大小和单双用1|2表示
		/// </summary>
		public int BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// 押单个选项多少份
		/// </summary>
		public int BuyCount
		{
			set{ _buycount=value;}
			get{return _buycount;}
		}
		/// <summary>
		/// 押多少币
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// 赢多少币
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态 0未开奖|1已开奖|2已兑奖
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 状态 0金币|1充值币
		/// </summary>
		public int bzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 押注时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

