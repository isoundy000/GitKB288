using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// 实体类DzpkCard 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DzpkCard
	{
		public DzpkCard()
		{}
		#region Model
		private int _id;
		private int _rmid;
		private int? _pokersuit;
		private int? _pokerrank;
		/// <summary>
		/// 扑克牌ID，自动生成
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
		/// 花色 0代表方块, 1代表梅花, 2代表红桃, 3代表黑桃
		/// </summary>
		public int? PokerSuit
		{
			set{ _pokersuit=value;}
			get{return _pokersuit;}
		}
		/// <summary>
		/// 点数  2代表2, 3代表3, 4代表4
		/// </summary>
		public int? PokerRank
		{
			set{ _pokerrank=value;}
			get{return _pokerrank;}
		}
		#endregion Model

	}
}

