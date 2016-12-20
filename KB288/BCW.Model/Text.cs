using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Text ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Text
	{
		public Text()
		{}
		#region Model
		private int _id;
		private int _forumid;
		private int _types;
		private int _labelid;
		private string _title;
		private string _content;
		private string _hidecontent;
		private int _usid;
		private string _usname;
		private int _replynum;
		private string _replyid;
		private int _readnum;
		private int _isgood;
		private int _isrecom;
		private int _islock;
		private int _istop;
		private long _prices;
		private int _price;
		private int _price2;
		private long _pricel;
		private int _bztype;
		private int _hidetype;
		private string _payid;
		private string _payci;
		private int _isseen;
		private int _isover;
		private int _isdel;
		private string _restats;
		private string _relist;
		private int _filenum;
		private int _tsid;
		private int _isflow;
		private DateTime _addtime;
		private DateTime _retime;
		private DateTime _flowtime;
        private DateTime _praisetime;
		private int _gaddnum;
		private int _gwinnum;
		private int _glznum;
        private int _gmnum;
        private int _gqinum;
        private int _praise;
        private string __praiseid;
        private string _PricesLimit;
        private string _IsPriceID;
        private int _istxt;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// �ɱҸ��ԣ��ظ��Ÿ��Բ��ɱ�
        /// </summary>
        public string PricesLimit
        {
            set { _PricesLimit = value; }
            get { return _PricesLimit; }
        }
        /// <summary>
        /// ������ Ĭ��Ϊ0
        /// </summary>
        public int Praise
        {
            set { _praise = value; }
            get { return _praise; }
        }
        /// <summary>
        /// ���ɱ�ID
        /// </summary>
        public string IsPriceID
        {
            set { _IsPriceID = value; }
            get { return _IsPriceID; }
        }
        /// <summary>
        /// �����˵�ID
        /// </summary>
        public string PraiseID
        {
            set { __praiseid = value; }
            get { return __praiseid; }
        }
        /// <summary>
        /// ���µ���ʱ��
        /// </summary>
        public DateTime PraiseTime
        {
            set { _praisetime = value; }
            get { return _praisetime; }
        }
		/// <summary>
		/// ������̳ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ���ӱ�ǩ����
		/// </summary>
		public int LabelId
		{
			set{ _labelid=value;}
			get{return _labelid;}
		}
		/// <summary>
		/// ���ӱ���
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
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
		/// ��������
		/// </summary>
		public string HideContent
		{
			set{ _hidecontent=value;}
			get{return _hidecontent;}
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
		/// �ظ���
		/// </summary>
		public int ReplyNum
		{
			set{ _replynum=value;}
			get{return _replynum;}
		}
		/// <summary>
		/// �ظ�ID
		/// </summary>
		public string ReplyID
		{
		    set {_replyid=value;}
		    get {return _replyid;}
		}
		/// <summary>
		/// �����
		/// </summary>
		public int ReadNum
		{
			set{ _readnum=value;}
			get{return _readnum;}
		}
		/// <summary>
		/// �Ƿ񾫻�
		/// </summary>
		public int IsGood
		{
			set{ _isgood=value;}
			get{return _isgood;}
		}
		/// <summary>
		/// �Ƿ��Ƽ�
		/// </summary>
		public int IsRecom
		{
			set{ _isrecom=value;}
			get{return _isrecom;}
		}
		/// <summary>
		/// �Ƿ�������
		/// </summary>
		public int IsLock
		{
			set{ _islock=value;}
			get{return _islock;}
		}
		/// <summary>
		/// �Ƿ��ö����߹̵�
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// �ɱ��ܶ�
		/// </summary>
		public long Prices
		{
		    set {_prices=value;}
		    get {return _prices;}
		}
		/// <summary>
		/// �շѻ�ÿ����С�ɱ�����ȼ��ɼ��ĵȼ� 
		/// </summary>
		public int Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ÿ������ɱ���
		/// </summary>
		public int Price2
		{
			set{ _price2=value;}
			get{return _price2;}
		}
		/// <summary>
		/// ���ɱҶ���
		/// </summary>
		public long Pricel
		{
		    set {_pricel=value;}
		    get {return _pricel;}
		}
		/// <summary>
		/// �ɱұ���
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// �����������ʣ�0Ĭ�ϣ�1�ظ��ɼ���2���ѿɼ���
		/// </summary>
		public int HideType
		{
			set{ _hidetype=value;}
			get{return _hidetype;}
		}
		/// <summary>
		/// �����շ���ID
		/// </summary>
		public string PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// �ɱ�¥��β���������#�ֿ���0Ϊ���ޣ�
		/// </summary>
		public string PayCi
		{
			set{ _payci=value;}
			get{return _payci;}
		}
		/// <summary>
		/// 0����/1��¼�ɼ�/2�ֻ��ɼ�/3�ȼ��ɼ�
		/// </summary>
		public int IsSeen
		{
			set{ _isseen=value;}
			get{return _isseen;}
		}
		/// <summary>
		/// �Ƿ��ѽ���
		/// </summary>
		public int IsOver
		{
			set{ _isover=value;}
			get{return _isover;}
		}
		/// <summary>
		/// �Ƿ���ɾ��
		/// </summary>
		public int IsDel
		{
			set{ _isdel=value;}
			get{return _isdel;}
		}
		/// <summary>
		/// ��ϸ����
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// ����ID�б�
		/// </summary>
		public string ReList
		{
			set{ _relist=value;}
			get{return _relist;}
		}
		/// <summary>
		/// �����ļ���
		/// </summary>
		public int FileNum
		{
			set{ _filenum=value;}
			get{return _filenum;}
		}
		/// <summary>
		/// ר���ʶID
		/// </summary>
		public int TsID
		{
			set{ _tsid=value;}
			get{return _tsid;}
		}
		/// <summary>
		/// �Ƿ������1����/2ȫ��������
		/// </summary>
		public int IsFlow
		{
			set{ _isflow=value;}
			get{return _isflow;}
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
		/// �ظ�ʱ��
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// ������ֹʱ��
		/// </summary>
		public DateTime FlowTime
		{
			set{ _flowtime=value;}
			get{return _flowtime;}
		}
		/// <summary>
		/// ����̳:�������
		/// </summary>
		public int Gaddnum
		{
			set{ _gaddnum=value;}
			get{return _gaddnum;}
		}
		/// <summary>
		/// ����̳:�н�����
		/// </summary>
		public int Gwinnum
		{
			set{ _gwinnum=value;}
			get{return _gwinnum;}
		}
		/// <summary>
		/// ����̳:���д���
		/// </summary>
		public int Glznum
		{
			set{ _glznum=value;}
			get{return _glznum;}
		}
        /// <summary>
        /// ����̳:���д���
        /// </summary>
        public int Gmnum
        {
            set { _gmnum = value; }
            get { return _gmnum; }
        }
        /// <summary>
        /// ����̳:��������
        /// </summary>
        public int Gqinum
        {
            set { _gqinum = value; }
            get { return _gqinum; }
        }
        /// <summary>
        /// �Ƿ��Ѳɼ�TXT
        /// </summary>
        public int Istxt
        {
            set { _istxt = value; }
            get { return _istxt; }
        }
		#endregion Model

	}
}

