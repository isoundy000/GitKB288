using System;
namespace BCW.XinKuai3.Model
{
	/// <summary>
	/// ʵ����XK3_Bet ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class XK3_Bet
	{
		public XK3_Bet()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _play_way;
		private string _sum;
		private string _three_same_all;
		private string _three_same_single;
		private string _three_same_not;
		private string _three_continue_all;
		private string _two_same_all;
		private string _two_same_single;
		private string _two_dissame;
		private DateTime _input_time;
		private string _dantuo;
		private int _zhu;
		private long _getmoney;
		private long _putgold;
		private int _state;
		private string _lottery_issue;
		private long _zhu_money;
		private string _daxiao;
		private string _danshuang;
		private decimal _odds;
        private int _aa;
        private long _bb;
        private int _isRobot;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        //===================================
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }
        public long bb
        {
            set { _bb = value; }
            get { return _bb; }
        }
        public int isRobot
        {
            set { _isRobot = value; }
            get { return _isRobot; }
        }
        //===================================
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �淨
		/// </summary>
		public int Play_Way
		{
			set{ _play_way=value;}
			get{return _play_way;}
		}
		/// <summary>
		/// ��ֵ
		/// </summary>
		public string Sum
		{
			set{ _sum=value;}
			get{return _sum;}
		}
		/// <summary>
		/// ��ͬ��ͨѡ
		/// </summary>
		public string Three_Same_All
		{
			set{ _three_same_all=value;}
			get{return _three_same_all;}
		}
		/// <summary>
		/// ��ͬ�ŵ�ѡ
		/// </summary>
		public string Three_Same_Single
		{
			set{ _three_same_single=value;}
			get{return _three_same_single;}
		}
		/// <summary>
		/// ����ͬ��
		/// </summary>
		public string Three_Same_Not
		{
			set{ _three_same_not=value;}
			get{return _three_same_not;}
		}
		/// <summary>
		/// ������ͨѡ
		/// </summary>
		public string Three_Continue_All
		{
			set{ _three_continue_all=value;}
			get{return _three_continue_all;}
		}
		/// <summary>
		/// ��ͬ�Ÿ�ѡ
		/// </summary>
		public string Two_Same_All
		{
			set{ _two_same_all=value;}
			get{return _two_same_all;}
		}
		/// <summary>
		/// ��ͬ�ŵ�ѡ
		/// </summary>
		public string Two_Same_Single
		{
			set{ _two_same_single=value;}
			get{return _two_same_single;}
		}
		/// <summary>
		/// ����ͬ��
		/// </summary>
		public string Two_dissame
		{
			set{ _two_dissame=value;}
			get{return _two_dissame;}
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
		/// �Ƿ���
		/// </summary>
		public string DanTuo
		{
			set{ _dantuo=value;}
			get{return _dantuo;}
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
		/// ����׬ȡ�Ŀ��
		/// </summary>
		public long GetMoney
		{
			set{ _getmoney=value;}
			get{return _getmoney;}
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
		/// ��ʷ��ע(0��ʾδ����1��ʾ�н���2��ʾ����)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// �����ں�
		/// </summary>
		public string Lottery_issue
		{
			set{ _lottery_issue=value;}
			get{return _lottery_issue;}
		}
		/// <summary>
		/// ÿסͶ�Ŀ��
		/// </summary>
		public long Zhu_money
		{
			set{ _zhu_money=value;}
			get{return _zhu_money;}
		}
		/// <summary>
		/// ��С
		/// </summary>
		public string DaXiao
		{
			set{ _daxiao=value;}
			get{return _daxiao;}
		}
		/// <summary>
		/// ��˫
		/// </summary>
		public string DanShuang
		{
			set{ _danshuang=value;}
			get{return _danshuang;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public decimal Odds
		{
			set{ _odds=value;}
			get{return _odds;}
		}
		#endregion Model

	}
}

