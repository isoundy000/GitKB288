using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Horsepay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Horsepay
	{
		public Horsepay()
		{}
		#region Model
		private int _id;
		private int _bztype;
		private int _types;
		private int _horseid;
		private int _usid;
		private string _usname;
		private long _buycent;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int bzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int HorseId
		{
			set{ _horseid=value;}
			get{return _horseid;}
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
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ��ע��
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
		/// <summary>
		/// Ӯ�Ҷ�
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ״̬0δ��/1�ѿ���/2�Ѷҽ�
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ��עʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

