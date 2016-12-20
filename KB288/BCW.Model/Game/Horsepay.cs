using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Horsepay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Horsepay
	{
		public Horsepay()
		{}
		#region Model
		private int _id;
		private int _bztype;
		private int _types;
		private int _horseid;
		private int _usid;
		private string _usname;
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
		/// 局数ID
		/// </summary>
		public int HorseId
		{
			set{ _horseid=value;}
			get{return _horseid;}
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
		/// 状态0未开/1已开奖/2已兑奖
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

