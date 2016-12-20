using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// 实体类JQC_Jackpot 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class JQC_Jackpot
	{
		public JQC_Jackpot()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _inprize;
		private long _outprize;
		private long _jackpot;
		private DateTime _addtime;
		private int _phase;
		private int _type;
		private int _betid;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户名
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 投进的
		/// </summary>
		public long InPrize
		{
			set{ _inprize=value;}
			get{return _inprize;}
		}
		/// <summary>
		/// 派奖的
		/// </summary>
		public long OutPrize
		{
			set{ _outprize=value;}
			get{return _outprize;}
		}
		/// <summary>
		/// 奖池
		/// </summary>
		public long Jackpot
		{
			set{ _jackpot=value;}
			get{return _jackpot;}
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
		/// 期号
		/// </summary>
		public int phase
		{
			set{ _phase=value;}
			get{return _phase;}
		}
        /// <summary>
        /// 0玩家下注1派奖2退回3系统收取4系统增加
        /// </summary>
        public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 投注ID
		/// </summary>
		public int BetID
		{
			set{ _betid=value;}
			get{return _betid;}
		}
		#endregion Model

	}
}

