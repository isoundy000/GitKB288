using System;
namespace BCW.ssc.Model
{
	/// <summary>
	/// ʵ����SSCpay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SSCpay
	{
		public SSCpay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _sscid;
		private int _usid;
		private string _usname;
		private long _price;
		private int _icount;
		private string _notes;
		private string _result;
		private long _prices;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
		private string _winnotes;
		private int _isspier;
        private decimal _odds;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
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
		public int SSCId
		{
			set{ _sscid=value;}
			get{return _sscid;}
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
		/// ÿע���
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ����ע
		/// </summary>
		public int iCount
		{
			set{ _icount=value;}
			get{return _icount;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// �������:1 2 3 4 5
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// ��ע�ܽ��
		/// </summary>
		public long Prices
		{
			set{ _prices=value;}
			get{return _prices;}
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
		/// ״̬(0δ������1�ѿ�����2�Ѷҽ�)
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
		/// <summary>
		/// Ӯ������
		/// </summary>
		public string WinNotes
		{
			set{ _winnotes=value;}
			get{return _winnotes;}
		}
		/// <summary>
		/// �����ˣ�0�ǡ�1�ǣ�
		/// </summary>
		public int IsSpier
		{
			set{ _isspier=value;}
			get{return _isspier;}
		}
        /// <summary>
        /// ����
        /// </summary>
        public decimal Odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
		#endregion Model

	}
}

