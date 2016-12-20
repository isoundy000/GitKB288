using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// 实体类DzpkHistory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DzpkHistory
	{
		public DzpkHistory()
		{}
		#region Model
		private int _id;
		private int _rmid;
		private int _usid;
		private int _rankchk;
		private string _pokercards;
		private string _pokerchips;
		private DateTime _timeout;
		private string _winner;
		private string _rmmake;
		private long _getmoney;
		private int? _ispayout;
		/// <summary>
		/// 历史ID，自动生成
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 房间ID 对应房间ID
		/// </summary>
		public int RmID
		{
			set{ _rmid=value;}
			get{return _rmid;}
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
		/// 下注流程标记0为庄家，1为小盲，2为大盲，顺序递加，不超过房间上限
		/// </summary>
		public int RankChk
		{
			set{ _rankchk=value;}
			get{return _rankchk;}
		}
		/// <summary>
		/// 数组方式记录每一轮发牌的详细信息用逗号分隔，P代表弃牌
		/// </summary>
		public string PokerCards
		{
			set{ _pokercards=value;}
			get{return _pokercards;}
		}
		/// <summary>
		/// 每轮下注的筹码数，数字代表加注数，0代表过，P代表弃牌
		/// </summary>
		public string PokerChips
		{
			set{ _pokerchips=value;}
			get{return _pokerchips;}
		}
		/// <summary>
		/// 最后的下注时间，叠加上房间的操作时间，超出即视为无操作，剔除出房间
		/// </summary>
		public DateTime TimeOut
		{
			set{ _timeout=value;}
			get{return _timeout;}
		}
		/// <summary>
		/// 历史判定记录，方便核查算法错对
		/// </summary>
		public string Winner
		{
			set{ _winner=value;}
			get{return _winner;}
		}
		/// <summary>
		/// 分类标记，历史分类统计用
		/// </summary>
		public string RmMake
		{
			set{ _rmmake=value;}
			get{return _rmmake;}
		}
		/// <summary>
		/// 得到的钱币数
		/// </summary>
		public long GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
		}
		/// <summary>
		/// 是否派彩 0否 1是
		/// </summary>
		public int? IsPayOut
		{
			set{ _ispayout=value;}
			get{return _ispayout;}
		}
		#endregion Model

	}
}

