using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����HcPay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class HcPay
	{
		public HcPay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _cid;
		private int _usid;
		private string _usname;
		private string _vote;
		private long _paycent;
        private long _paycents;
		private string _result;
		private int _state;
		private long _wincent;
		private int _bztype;
		private DateTime _addtime;
		private int _isspier;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
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
		/// ����
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
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
		/// �ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// Ͷע����
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// ��ע��
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
        /// <summary>
        /// ��ע�ܶ�
        /// </summary>
        public long PayCents
        {
            set { _paycents = value; }
            get { return _paycents; }
        }
		/// <summary>
		/// �������
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// ״̬��0δ����\1�ѿ�����
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// Ӯȡ���
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ���� 
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �Ƿ������
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
		#endregion Model

	}
}

