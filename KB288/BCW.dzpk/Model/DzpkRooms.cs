using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// 实体类DzpkRooms 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DzpkRooms
	{
		public DzpkRooms()
		{}
		#region Model
		private int _id;
		private string _drname;
		private int _drtype;
		private int _owers;
		private string _passwd;
		private long _gsmallb;
		private long _gbigb;
		private long _gminb;
		private long _gmaxb;
		private long _gsercharge;
		private long _gserchargeall;
		private int _gmaxuser;
		private long _gsidepot;
		private int _gsettime;
		private int _gactid;
		private string _gactbetid;
		private DateTime? _lasttime;
		private long _lastrank;
		/// <summary>
		/// 房间ID，自动生成
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 名称，默认：00桌（房）
		/// </summary>
		public string DRName
		{
			set{ _drname=value;}
			get{return _drname;}
		}
		/// <summary>
		/// 房间类型：1初级、2中级、3高级
		/// </summary>
		public int DRType
		{
			set{ _drtype=value;}
			get{return _drtype;}
		}
		/// <summary>
		/// 房主，第一个进入的玩家，或房主退出后其他的玩家，没有则清空
		/// </summary>
		public int Owers
		{
			set{ _owers=value;}
			get{return _owers;}
		}
		/// <summary>
		/// 房间密码，由房主设定，默认为空，任何人都能进入
		/// </summary>
		public string PassWD
		{
			set{ _passwd=value;}
			get{return _passwd;}
		}
		/// <summary>
		///  小盲注
		/// </summary>
		public long GSmallb
		{
			set{ _gsmallb=value;}
			get{return _gsmallb;}
		}
		/// <summary>
		/// 大盲注
		/// </summary>
		public long GBigb
		{
			set{ _gbigb=value;}
			get{return _gbigb;}
		}
		/// <summary>
		/// 最小资金值 进入该房间最少的持币值
		/// </summary>
		public long GMinb
		{
			set{ _gminb=value;}
			get{return _gminb;}
		}
		/// <summary>
		/// 最大金币值，每次入房可操作的最大币值
		/// </summary>
		public long GMaxb
		{
			set{ _gmaxb=value;}
			get{return _gmaxb;}
		}
		/// <summary>
		/// 手续费费率
		/// </summary>
		public long GSerCharge
		{
			set{ _gsercharge=value;}
			get{return _gsercharge;}
		}
		/// <summary>
		/// 总共扣除的手续费
		/// </summary>
		public long GSerChargeALL
		{
			set{ _gserchargeall=value;}
			get{return _gserchargeall;}
		}
		/// <summary>
		/// 最大玩家数
		/// </summary>
		public int GMaxUser
		{
			set{ _gmaxuser=value;}
			get{return _gmaxuser;}
		}
		/// <summary>
		/// 奖池总金额
		/// </summary>
		public long GSidePot
		{
			set{ _gsidepot=value;}
			get{return _gsidepot;}
		}
		/// <summary>
		/// 操作时间，默认10或15秒
		/// </summary>
		public int GSetTime
		{
			set{ _gsettime=value;}
			get{return _gsettime;}
		}
		/// <summary>
		/// 发牌流程序号1为首轮，其次2/3/4/5 对应流程执行动作
		/// </summary>
		public int GActID
		{
			set{ _gactid=value;}
			get{return _gactid;}
		}
		/// <summary>
		/// 下注标记，默认Z 即本轮不需要下注，为A时不执行发牌流程，等待下载完毕后继续
		/// </summary>
		public string GActBetID
		{
			set{ _gactbetid=value;}
			get{return _gactbetid;}
		}
		/// <summary>
		/// 最后一次游戏结算时间
		/// </summary>
		public DateTime? LastTime
		{
			set{ _lasttime=value;}
			get{return _lasttime;}
		}
		/// <summary>
		/// 最少下注数
		/// </summary>
		public long LastRank
		{
			set{ _lastrank=value;}
			get{return _lastrank;}
		}
		#endregion Model

	}
}

