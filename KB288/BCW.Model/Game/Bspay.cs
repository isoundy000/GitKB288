using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Bspay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Bspay
	{
		public Bspay()
		{}
		#region Model
		private int _id;
		private int _bsid;
		private string _bstitle;
		private int _usid;
		private string _usname;
		private int _bettype;
		private long _paycent;
		private long _wincent;
		private int _bztype;
		private DateTime _addtime;
		private string _usip;
		private string _usua;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 游戏ID
		/// </summary>
		public int BsId
		{
			set{ _bsid=value;}
			get{return _bsid;}
		}
		/// <summary>
		/// 庄名
		/// </summary>
		public string BsTitle
		{
			set{ _bstitle=value;}
			get{return _bstitle;}
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
		/// 下注类型
		/// </summary>
		public int BetType
		{
			set{ _bettype=value;}
			get{return _bettype;}
		}
		/// <summary>
		/// 下注数量
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 赢取数量
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 币种（0虚拟币/1元）
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
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
		/// 用户IP
		/// </summary>
		public string UsIP
		{
			set{ _usip=value;}
			get{return _usip;}
		}
		/// <summary>
		/// 用户UA
		/// </summary>
		public string UsUA
		{
			set{ _usua=value;}
			get{return _usua;}
		}
		#endregion Model

	}
}

