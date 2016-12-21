using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// ʵ����DzpkCard ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
		/// �˿���ID���Զ�����
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID ��Ӧ����ID
		/// </summary>
		public int RmID
		{
			set{ _rmid=value;}
			get{return _rmid;}
		}
		/// <summary>
		/// ��ɫ 0������, 1����÷��, 2�������, 3�������
		/// </summary>
		public int? PokerSuit
		{
			set{ _pokersuit=value;}
			get{return _pokersuit;}
		}
		/// <summary>
		/// ����  2����2, 3����3, 4����4
		/// </summary>
		public int? PokerRank
		{
			set{ _pokerrank=value;}
			get{return _pokerrank;}
		}
		#endregion Model

	}
}

