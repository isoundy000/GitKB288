using System;
namespace BCW.SFC.Model
{
	/// <summary>
	/// ʵ����SfList ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SfList
	{
		public SfList()
		{}
		#region Model
		private int _id;
		private int _cid;
		private string _match;
		private string _team_home;
		private string _team_away;
		private string _start_time;
		private string _score;
		private string _result;
		private int _state;
		private long _paycent;
		private int _paycount;
		private DateTime _endtime;
		private string _other;
        private DateTime _sale_starttime;
        private long _nowprize;
        private long _nextprize;
        private long _sysprize;
        private int _sysprizestatue;
        private long _sysdayprize;
        /// <summary>
        /// 
        /// </summary>
        public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �ں�
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Match
		{
			set{ _match=value;}
			get{return _match;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public string Team_Home
		{
			set{ _team_home=value;}
			get{return _team_home;}
		}
		/// <summary>
		/// �ͳ����
		/// </summary>
		public string Team_Away
		{
			set{ _team_away=value;}
			get{return _team_away;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string Start_time
		{
			set{ _start_time=value;}
			get{return _start_time;}
		}
		/// <summary>
		/// �ȷ�
		/// </summary>
		public string Score
		{
			set{ _score=value;}
			get{return _score;}
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
		/// �Ƿ񿪽���0,1��
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// Ͷע�ܶ�
		/// </summary>
		public long PayCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// Ͷע��
		/// </summary>
		public int PayCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// ��ֹʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string other
		{
			set{ _other=value;}
			get{return _other;}
		}
        /// <summary>
        /// Ͷע��ʼʱ��
        /// </summary>
        public DateTime Sale_StartTime
        {
            set { _sale_starttime = value; }
            get { return _sale_starttime; }
        }
        /// <summary>
        /// ��ǰ�����ܶ�
        /// </summary>
        public long nowprize
        {
            set { _nowprize = value; }
            get { return _nowprize; }
        }
        /// <summary>
        /// ���ཱ��
        /// </summary>
        public long nextprize
        {
            set { _nextprize = value; }
            get { return _nextprize; }
        }
        /// <summary>
        /// ϵͳͶ��
        /// </summary>
        public long sysprize
        {
            set { _sysprize = value; }
            get { return _sysprize; }
        }
        /// <summary>
        /// ϵͳͶ����շ�
        /// </summary>
        public int sysprizestatue
        {
            set { _sysprizestatue = value; }
            get { return _sysprizestatue; }
        }
        /// <summary>
        /// ÿ��ϵͳ��ȡ����
        /// </summary>
        public long sysdayprize
        {
            set { _sysdayprize = value; }
            get { return _sysdayprize; }
        }
        #endregion Model

    }
}

