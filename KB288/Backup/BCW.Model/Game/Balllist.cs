using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Balllist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Balllist
	{
		public Balllist()
		{}
		#region Model
		private int _id;
		private int _winnum;
		private int _outnum;
		private int _addnum;
		private int _icent;
		private int _odds;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _pool;
		private long _beforepool;
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
		/// 开奖号码
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// 卖出份数
		/// </summary>
		public int OutNum
		{
			set{ _outnum=value;}
			get{return _outnum;}
		}
		/// <summary>
		/// 已卖出份数
		/// </summary>
		public int AddNum
		{
			set{ _addnum=value;}
			get{return _addnum;}
		}
		/// <summary>
		/// 每份币数
		/// </summary>
		public int iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// 每份币数
		/// </summary>
		public int Odds
		{
			set{ _odds=value;}
			get{return _odds;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// 开奖时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 奖池
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// 落下奖池
		/// </summary>
		public long BeforePool
		{
			set{ _beforepool=value;}
			get{return _beforepool;}
		}
		/// <summary>
		/// 状态（0进行中/1已结束）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

