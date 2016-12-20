using System;
namespace BCW.QuickBet.Model
{
	/// <summary>
	/// 实体类QuickBet 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class QuickBet
	{
		public QuickBet()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _game;
		private string _bet;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 游戏
		/// </summary>
		public string Game
		{
			set{ _game=value;}
			get{return _game;}
		}
		/// <summary>
		/// 快捷下注
		/// </summary>
		public string Bet
		{
			set{ _bet=value;}
			get{return _bet;}
		}
		#endregion Model

	}
}

