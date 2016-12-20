using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Marry ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Marry
	{
		public Marry()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _ussex;
		private int _reid;
		private string _rename;
		private int _resex;
		private string _oath;
		private string _oath2;
		private int _isparty;
		private DateTime _addtime;
		private int _acusid;
		private DateTime _actime;
		private int _state;
        private string _lovestat;
		private string _homename;
		private int _flownum;
		private int _homeclick;
        private string _flowstat;
        private string _flowtimes;
        private string _marrypk;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ״̬��0������/1���/2��飩
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �����ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ������ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ������Ա�
		/// </summary>
		public int UsSex
		{
			set{ _ussex=value;}
			get{return _ussex;}
		}
		/// <summary>
		/// �����ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// ������ǳ�
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// ������Ա�
		/// </summary>
		public int ReSex
		{
			set{ _resex=value;}
			get{return _resex;}
		}
		/// <summary>
		/// ���������
		/// </summary>
		public string Oath
		{
			set{ _oath=value;}
			get{return _oath;}
		}
		/// <summary>
		/// ���ԭ��(���Ů����)
		/// </summary>
		public string Oath2
		{
			set{ _oath2=value;}
			get{return _oath2;}
		}
		/// <summary>
		/// �Ƿ��Ѱ���ϯ
		/// </summary>
		public int IsParty
		{
			set{ _isparty=value;}
			get{return _isparty;}
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
		/// ����ID
		/// </summary>
		public int AcUsID
		{
			set{ _acusid=value;}
			get{return _acusid;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
		}
		/// <summary>
		/// ���������״̬
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
        /// <summary>
        /// ��������ͳ��
        /// </summary>
        public string LoveStat
        {
            set { _lovestat = value; }
            get { return _lovestat; }
        }
		/// <summary>
		/// ��԰����
		/// </summary>
		public string HomeName
		{
			set{ _homename=value;}
			get{return _homename;}
		}
		/// <summary>
		/// õ������
		/// </summary>
		public int FlowNum
		{
			set{ _flownum=value;}
			get{return _flownum;}
		}
		/// <summary>
		/// ��԰����
		/// </summary>
		public int HomeClick
		{
			set{ _homeclick=value;}
			get{return _homeclick;}
		}
        /// <summary>
        /// �ʻ�ͳ��
        /// </summary>
        public string FlowStat
        {
            set { _flowstat = value; }
            get { return _flowstat; }
        }
        /// <summary>
        /// ����ʱ�䣨������Ů˫����ʱ��洢��
        /// </summary>
        public string FlowTimes
        {
            set { _flowtimes = value; }
            get { return _flowtimes; }
        }
        /// <summary>
        /// ���֤��ַ
        /// </summary>
        public string MarryPk
        {
            set { _marrypk = value; }
            get { return _marrypk; }
        }
		#endregion Model

	}
}

