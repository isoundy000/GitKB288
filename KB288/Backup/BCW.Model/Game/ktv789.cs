using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����ktv789 ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ktv789
	{
		public ktv789()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _stname;
		private int _paycent;
		private string _oneusname;
		private int _oneusid;
		private string _twousname;
		private int _twousid;
		private string _thrusname;
		private int _thrusid;
		private int _expir;
		private string _oneshot;
		private string _twoshot;
		private string _thrshot;
		private DateTime? _onetime;
		private DateTime? _twotime;
		private DateTime? _thrtime;
		private int _pkcount;
		private int _online;
		private int _smallpay;
		private int _bigpay;
		private int _nextshot;
		private string _lines;
        private int _goldtype;
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
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string StName
		{
			set{ _stname=value;}
			get{return _stname;}
		}
		/// <summary>
		/// ��ע�Ҷ�
		/// </summary>
		public int PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// ��һ���û�
		/// </summary>
		public string OneUsName
		{
			set{ _oneusname=value;}
			get{return _oneusname;}
		}
		/// <summary>
		/// ��һ���û�ID
		/// </summary>
		public int OneUsId
		{
			set{ _oneusid=value;}
			get{return _oneusid;}
		}
		/// <summary>
		/// �ڶ����û�
		/// </summary>
		public string TwoUsName
		{
			set{ _twousname=value;}
			get{return _twousname;}
		}
		/// <summary>
		/// �ڶ����û�ID
		/// </summary>
		public int TwoUsId
		{
			set{ _twousid=value;}
			get{return _twousid;}
		}
		/// <summary>
		/// �������û�
		/// </summary>
		public string ThrUsName
		{
			set{ _thrusname=value;}
			get{return _thrusname;}
		}
		/// <summary>
		/// �������û�ID
		/// </summary>
		public int ThrUsId
		{
			set{ _thrusid=value;}
			get{return _thrusid;}
		}
		/// <summary>
		/// ������ʱʱ��
		/// </summary>
		public int Expir
		{
			set{ _expir=value;}
			get{return _expir;}
		}
		/// <summary>
		/// �û�1��ɫ���
		/// </summary>
		public string OneShot
		{
			set{ _oneshot=value;}
			get{return _oneshot;}
		}
		/// <summary>
		/// �û�2��ɫ���
		/// </summary>
		public string TwoShot
		{
			set{ _twoshot=value;}
			get{return _twoshot;}
		}
		/// <summary>
		/// �û�3��ɫ���
		/// </summary>
		public string ThrShot
		{
			set{ _thrshot=value;}
			get{return _thrshot;}
		}
		/// <summary>
		/// �û�1����ʱ��
		/// </summary>
		public DateTime? OneTime
		{
			set{ _onetime=value;}
			get{return _onetime;}
		}
		/// <summary>
		/// �û�2����ʱ��
		/// </summary>
		public DateTime? TwoTime
		{
			set{ _twotime=value;}
			get{return _twotime;}
		}
		/// <summary>
		/// �û�3����ʱ��
		/// </summary>
		public DateTime? ThrTime
		{
			set{ _thrtime=value;}
			get{return _thrtime;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int PkCount
		{
			set{ _pkcount=value;}
			get{return _pkcount;}
		}
		/// <summary>
		/// ������������
		/// </summary>
		public int Online
		{
			set{ _online=value;}
			get{return _online;}
		}
		/// <summary>
		/// �����ע��
		/// </summary>
		public int SmallPay
		{
			set{ _smallpay=value;}
			get{return _smallpay;}
		}
		/// <summary>
		/// ��С��ע��
		/// </summary>
		public int BigPay
		{
			set{ _bigpay=value;}
			get{return _bigpay;}
		}
		/// <summary>
		/// ��ע����ID��˭���ȿ�ɫ��Ȩ�ޣ�
		/// </summary>
		public int NextShot
		{
			set{ _nextshot=value;}
			get{return _nextshot;}
		}
		/// <summary>
		/// ����ͳ��
		/// </summary>
		public string Lines
		{
			set{ _lines=value;}
			get{return _lines;}
		}
        /// <summary>
        /// �Ƿ�����棨0����/1�⣩
        /// </summary>
        public int GoldType
        {
            set { _goldtype = value; }
            get { return _goldtype; }
        }
		#endregion Model

	}
}

