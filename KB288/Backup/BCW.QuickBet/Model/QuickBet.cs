using System;
namespace BCW.QuickBet.Model
{
	/// <summary>
	/// ʵ����QuickBet ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
		/// �û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ��Ϸ
		/// </summary>
		public string Game
		{
			set{ _game=value;}
			get{return _game;}
		}
		/// <summary>
		/// �����ע
		/// </summary>
		public string Bet
		{
			set{ _bet=value;}
			get{return _bet;}
		}
		#endregion Model

	}
}

