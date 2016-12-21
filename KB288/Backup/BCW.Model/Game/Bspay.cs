using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Bspay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Bspay
	{
		public Bspay()
		{}
		#region Model
		private int _id;
		private int _bsid;
		private string _bstitle;
		private int _usid;
		private string _usname;
		private int _bettype;
		private long _paycent;
		private long _wincent;
		private int _bztype;
		private DateTime _addtime;
		private string _usip;
		private string _usua;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��ϷID
		/// </summary>
		public int BsId
		{
			set{ _bsid=value;}
			get{return _bsid;}
		}
		/// <summary>
		/// ׯ��
		/// </summary>
		public string BsTitle
		{
			set{ _bstitle=value;}
			get{return _bstitle;}
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
		/// ��ע����
		/// </summary>
		public int BetType
		{
			set{ _bettype=value;}
			get{return _bettype;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// Ӯȡ����
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ���֣�0�����/1Ԫ��
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ��עʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �û�IP
		/// </summary>
		public string UsIP
		{
			set{ _usip=value;}
			get{return _usip;}
		}
		/// <summary>
		/// �û�UA
		/// </summary>
		public string UsUA
		{
			set{ _usua=value;}
			get{return _usua;}
		}
		#endregion Model

	}
}

