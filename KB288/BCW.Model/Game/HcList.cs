using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����HcList ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class HcList
	{
		public HcList()
		{}
		#region Model
		private int _id;
		private int _cid;
		private string _result;
		private string _notes;
		private int _state;
        private long _paycent;
        private int _paycount;
        private long _paycent2;
        private int _paycount2;
		private DateTime _endtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int CID
		{
			set{ _cid=value;}
			get{return _cid;}
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
		/// ����
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ״̬��0δ����/1�ѿ�����
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
        /// <summary>
        /// ��ע�ܶ�
        /// </summary>
        public long payCent
        {
            set { _paycent = value; }
            get { return _paycent; }
        }
        /// <summary>
        /// ��ע��ע��
        /// </summary>
        public int payCount
        {
            set { _paycount = value; }
            get { return _paycount; }
        }
        /// <summary>
        /// ��ע�ܶ�2
        /// </summary>
        public long payCent2
        {
            set { _paycent2 = value; }
            get { return _paycent2; }
        }
        /// <summary>
        /// ��ע��ע��2
        /// </summary>
        public int payCount2
        {
            set { _paycount2 = value; }
            get { return _paycount2; }
        }
		/// <summary>
		/// ��ֹ��עʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

