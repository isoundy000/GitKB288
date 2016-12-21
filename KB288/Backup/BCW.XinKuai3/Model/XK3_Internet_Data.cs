using System;
namespace BCW.XinKuai3.Model
{
	/// <summary>
	/// ʵ����XK3_Internet_Data ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class XK3_Internet_Data
	{
		public XK3_Internet_Data()
		{}
		#region Model
		private int _id;
		private string _lottery_issue;
		private string _lottery_num;
		private DateTime _lottery_time;
		private DateTime _updatetime;
		private string _sum;
		private string _three_same_all;
		private string _three_same_single;
		private string _three_same_not;
		private string _three_continue_all;
		private string _two_same_all;
		private string _two_same_single;
		private string _two_dissame;
		private string _daxiao;
		private string _danshuang;
        private int _aa;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        //=========================================
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }

        //=========================================
		/// <summary>
		/// �����ں�
		/// </summary>
		public string Lottery_issue
		{
			set{ _lottery_issue=value;}
			get{return _lottery_issue;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Lottery_num
		{
			set{ _lottery_num=value;}
			get{return _lottery_num;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime Lottery_time
		{
			set{ _lottery_time=value;}
			get{return _lottery_time;}
		}
		/// <summary>
		/// ��������ʱ��
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
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
		#endregion Model

	}
}

