using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_WinnersGame ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_WinnersGame
	{
		public tb_WinnersGame()
		{}
		#region Model
		private int _id;
		private string _gamename;
		private long _price;
		private int? _ptype;
		private string _ident;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GameName
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ptype
		{
			set{ _ptype=value;}
			get{return _ptype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		#endregion Model

	}
}

