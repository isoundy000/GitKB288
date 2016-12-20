using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Stklist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Stklist
	{
		public Stklist()
		{}
		#region Model
		private int _id;
		private int _winnum;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _pool;
		private int _wincount;
		private long _winpool;
		private int _state;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 开出的上证指数
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// 开盘时间
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// 收盘时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 下注总额
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// 中奖人数
		/// </summary>
		public int WinCount
		{
			set{ _wincount=value;}
			get{return _wincount;}
		}
		/// <summary>
		/// 下注总额2
		/// </summary>
		public long WinPool
		{
			set{ _winpool=value;}
			get{return _winpool;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

