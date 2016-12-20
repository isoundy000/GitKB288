using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Stkpay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Stkpay
	{
		public Stkpay()
		{}
		#region Model
		private int _id;
		private int _bztype;
		private int _types;
		private int _winnum;
		private int _stkid;
		private int _usid;
		private string _usname;
		private decimal _odds;
		private long _buycent;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
        private int _isSpier;
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
		/// ��������
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// ��֤ID
		/// </summary>
		public int StkId
		{
			set{ _stkid=value;}
			get{return _stkid;}
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
		///  ����
		/// </summary>
		public decimal Odds
		{
			set{ _odds=value;}
			get{return _odds;}
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
		/// ״̬��0δ��/1�ѿ���/2�Ѷҽ���
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
        /// ״̬��0��Ա��ע/1��������ע��
        /// </summary>
        public int isSpier
        {
            set { _isSpier = value; }
            get { return _isSpier; }
        }
		#endregion Model

	}
}

