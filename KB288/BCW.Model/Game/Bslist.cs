using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Bslist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Bslist
	{
		public Bslist()
		{}
		#region Model
		private int _id;
		private string _title;
		private long _money;
		private long _smallpay;
		private long _bigpay;
		private int _usid;
		private string _usname;
		private int _click;
		private int _bztype;
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
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �ʽ���
		/// </summary>
		public long Money
		{
			set{ _money=value;}
			get{return _money;}
		}
		/// <summary>
		/// ��С��ע
		/// </summary>
		public long SmallPay
		{
			set{ _smallpay=value;}
			get{return _smallpay;}
		}
		/// <summary>
		/// �����ע
		/// </summary>
		public long BigPay
		{
			set{ _bigpay=value;}
			get{return _bigpay;}
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
		/// ����
		/// </summary>
		public int Click
		{
			set{ _click=value;}
			get{return _click;}
		}
		/// <summary>
		/// �������ͣ�0�����/1Ԫ��
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

