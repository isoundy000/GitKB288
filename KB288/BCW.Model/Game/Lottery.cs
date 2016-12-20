using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Lottery 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Lottery
	{
		public Lottery()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private long _paycent;
		private long _outcent;
		private string _outgift;
		private int _usid;
		private string _usname;
		private int _freshsec;
		private int _freshmin;
		private int _wincount;
		private DateTime _actime;
		private DateTime _addtime;
		private DateTime _endtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 抽奖标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 抽奖总金额
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 支出了多少
		/// </summary>
		public long OutCent
		{
			set{ _outcent=value;}
			get{return _outcent;}
		}
		/// <summary>
		/// 礼物项待用
		/// </summary>
		public string OutGift
		{
			set{ _outgift=value;}
			get{return _outgift;}
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
		/// 每局抽奖时间(秒)
		/// </summary>
		public int FreshSec
		{
			set{ _freshsec=value;}
			get{return _freshsec;}
		}
		/// <summary>
		/// 每局结束后N分钟后进行下一局
		/// </summary>
		public int FreshMin
		{
			set{ _freshmin=value;}
			get{return _freshmin;}
		}
		/// <summary>
		/// 中奖数
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
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
		/// 结束时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

