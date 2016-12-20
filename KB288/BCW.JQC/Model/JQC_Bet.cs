using System;
namespace BCW.JQC.Model
{
	/// <summary>
	/// ʵ����JQC_Bet ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class JQC_Bet
	{
		public JQC_Bet()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _lottery_issue;
		private string _votenum;
		private int _zhu;
		private int _zhu_money;
		private long _putgold;
		private long _getmoney;
		private int _state;
		private int _voterate;
		private DateTime _input_time;
		private int _isrobot;
        private int _a;
        private int _prize1;
        private int _prize2;
        private int _prize3;
        private int _prize4;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        public int a
        {
            set { _a = value; }
            get { return _a; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �����ں�
		/// </summary>
		public int Lottery_issue
		{
			set{ _lottery_issue=value;}
			get{return _lottery_issue;}
		}
		/// <summary>
		/// Ͷע����
		/// </summary>
		public string VoteNum
		{
			set{ _votenum=value;}
			get{return _votenum;}
		}
		/// <summary>
		/// Ͷעע��
		/// </summary>
		public int Zhu
		{
			set{ _zhu=value;}
			get{return _zhu;}
		}
		/// <summary>
		/// ��ע���
		/// </summary>
		public int Zhu_money
		{
			set{ _zhu_money=value;}
			get{return _zhu_money;}
		}
		/// <summary>
		/// ��Ͷע���
		/// </summary>
		public long PutGold
		{
			set{ _putgold=value;}
			get{return _putgold;}
		}
		/// <summary>
		/// ����׬ȡ�Ŀ��
		/// </summary>
		public long GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
		}
        /// <summary>
        /// 0δ��/1��/2����/3����/4����    
        /// </summary>
        public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// Ͷע����
		/// </summary>
		public int VoteRate
		{
			set{ _voterate=value;}
			get{return _voterate;}
		}
		/// <summary>
		/// Ͷעʱ��
		/// </summary>
		public DateTime Input_Time
		{
			set{ _input_time=value;}
			get{return _input_time;}
		}
		/// <summary>
		/// 0Ϊ��Ա1Ϊ������
		/// </summary>
		public int isRobot
		{
			set{ _isrobot=value;}
			get{return _isrobot;}
		}
        /// <summary>
        /// ��һ�Ƚ���ע��
        /// </summary>
        public int Prize1
        {
            set { _prize1 = value; }
            get { return _prize1; }
        }
        /// <summary>
        /// �ж��Ƚ���ע��
        /// </summary>
        public int Prize2
        {
            set { _prize2 = value; }
            get { return _prize2; }
        }
        /// <summary>
        /// �����Ƚ���ע��
        /// </summary>
        public int Prize3
        {
            set { _prize3 = value; }
            get { return _prize3; }
        }
        /// <summary>
        /// ���ĵȽ���ע��
        /// </summary>
        public int Prize4
        {
            set { _prize4 = value; }
            get { return _prize4; }
        }
        #endregion Model

    }
}

