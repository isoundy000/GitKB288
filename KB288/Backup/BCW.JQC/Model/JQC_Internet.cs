using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// ʵ����JQC_Internet ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class JQC_Internet
	{
		public JQC_Internet()
		{}
		#region Model
		private int _id;
		private int _phase;
		private string _match;
		private string _team_home;
		private string _team_away;
		private DateTime _sale_start;
		private DateTime _sale_end;
		private string _start_time;
		private string _score;
		private string _result;
        private int _nowprize;
        private string _zhu;
        private string _zhu_money;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �ں�
		/// </summary>
		public int phase
		{
			set{ _phase=value;}
			get{return _phase;}
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
		/// ���ۿ�ʼʱ��
		/// </summary>
		public DateTime Sale_Start
		{
			set{ _sale_start=value;}
			get{return _sale_start;}
		}
		/// <summary>
		/// ���۽�ֹʱ��
		/// </summary>
		public DateTime Sale_End
		{
			set{ _sale_end=value;}
			get{return _sale_end;}
		}
		/// <summary>
		/// ������ʼʱ��
		/// </summary>
		public string Start_Time
		{
			set{ _start_time=value;}
			get{return _start_time;}
		}
		/// <summary>
		/// �����ȷ�
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
        /// ��ǰ�ںŵĽ���
        /// </summary>
        public int nowprize
        {
            set { _nowprize = value; }
            get { return _nowprize; }
        }
        /// <summary>
        /// ����Ͷעע��
        /// </summary>
        public string zhu
        {
            set { _zhu = value; }
            get { return _zhu; }
        }
        /// <summary>
        /// ÿע���
        /// </summary>
        public string zhu_money
        {
            set { _zhu_money = value; }
            get { return _zhu_money; }
        }
        #endregion Model

    }
}

