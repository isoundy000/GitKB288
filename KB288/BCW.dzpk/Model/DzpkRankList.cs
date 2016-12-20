using System;
namespace BCW.dzpk.Model
{
	/// <summary>
	/// ʵ����DzpkRankList ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DzpkRankList
	{
		public DzpkRankList()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _getpot;
		private DateTime? _gtime;
		private string _rmmake;
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
		/// ����
		/// </summary>
		public long GetPot
		{
			set{ _getpot=value;}
			get{return _getpot;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime? Gtime
		{
			set{ _gtime=value;}
			get{return _gtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RmMake
		{
			set{ _rmmake=value;}
			get{return _rmmake;}
		}
		#endregion Model

	}
}

