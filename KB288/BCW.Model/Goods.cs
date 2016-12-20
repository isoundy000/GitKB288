using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Goods ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Goods
	{
		public Goods()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private int _usid;
		private bool _isad;
		private string _title;
		private string _keyword;
		private string _files;
		private string _cover;
		private string _config;
		private string _content;
		private string _mobile;
		private decimal _citymoney;
		private decimal _vipmoney;
		private int _sellcount;
		private int _stockcount;
		private int _paytype;
		private int _posttype;
		private string _postmoney;
		private string _restats;
		private string _relastip;
		private int _readcount;
		private int _paycount;
		private int _evcount;
		private int _recount;
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
		/// ��Ŀ����ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// ����ID/0Ϊϵͳ����
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �Ƿ���
		/// </summary>
		public bool IsAd
		{
			set{ _isad=value;}
			get{return _isad;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �ؼ���
		/// </summary>
		public string KeyWord
		{
			set{ _keyword=value;}
			get{return _keyword;}
		}
		/// <summary>
		/// ��ƷͼƬ
		/// </summary>
		public string Files
		{
			set{ _files=value;}
			get{return _files;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public string Cover
		{
			set{ _cover=value;}
			get{return _cover;}
		}
		/// <summary>
		/// ��Ʒ����/����
		/// </summary>
		public string Config
		{
			set{ _config=value;}
			get{return _config;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// ��ϵ��ʽ:QQ/TEL
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// �г��۸�
		/// </summary>
		public decimal CityMoney
		{
			set{ _citymoney=value;}
			get{return _citymoney;}
		}
		/// <summary>
		/// ��Ա�۸�
		/// </summary>
		public decimal VipMoney
		{
			set{ _vipmoney=value;}
			get{return _vipmoney;}
		}
		/// <summary>
		/// �ѳ�������
		/// </summary>
		public int SellCount
		{
		    set { _sellcount = value; }
		    get { return _sellcount; }
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int StockCount
		{
		    set { _stockcount = value; }
		    get { return _stockcount; }
		}
		/// <summary>
		/// �ͻ���ʽ0��������/1���潻��/2�ȸ���󷢿�
		/// </summary>
		public int PayType
		{
			set{ _paytype=value;}
			get{return _paytype;}
		}
		/// <summary>
		/// �������0/�����/1���/2��ʯ
		/// </summary>
		public int PostType
		{
			set{ _posttype=value;}
			get{return _posttype;}
		}
		/// <summary>
		/// �ʵ��ʷ�(����:15|���|20|EMS,�����̼Ұ���)
		/// </summary>
		public string PostMoney
		{
			set{ _postmoney=value;}
			get{return _postmoney;}
		}
		/// <summary>
		/// ������ϸ
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// ���ظ�IP
		/// </summary>
		public string ReLastIP
		{
		    set { _relastip = value; }
		    get { return _relastip; }
		}
		/// <summary>
		/// �����
		/// </summary>
		public int Readcount
		{
			set{ _readcount=value;}
			get{return _readcount;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public int Recount
		{
			set{ _recount=value;}
			get{return _recount;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int Paycount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int Evcount
		{
			set{ _evcount=value;}
			get{return _evcount;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

