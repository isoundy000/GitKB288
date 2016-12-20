using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// ʵ����BJL_Room ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class BJL_Room
	{
		public BJL_Room()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _total;
		private long _lowtotal;
		private string _title;
		private string _contact;
		private DateTime _addtime;
		private int _state;
		private long _zhui_total;
        private int _type;
        private long _total_now;
        private long _shouxufei;
        private int _click;
        private long _bigpay;
        private long _bigmoney;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// �ʳ�
		/// </summary>
		public long Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// ��Ͳʳ�
		/// </summary>
		public long LowTotal
		{
			set{ _lowtotal=value;}
			get{return _lowtotal;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string contact
		{
			set{ _contact=value;}
			get{return _contact;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 0��ׯ1��װ
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ׷��
		/// </summary>
		public long zhui_Total
		{
			set{ _zhui_total=value;}
			get{return _zhui_total;}
		}
        /// <summary>
        /// 0δ����1׼������
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// ʵʱ�ʳ�
        /// </summary>
        public long Total_Now
        {
            set { _total_now = value; }
            get { return _total_now; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public long shouxufei
        {
            set { _shouxufei = value; }
            get { return _shouxufei; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// ��߲ʳ�
        /// </summary>
        public long BigPay
        {
            set { _bigpay = value; }
            get { return _bigpay; }
        }
        /// <summary>
        /// ÿ��ÿID�����Ͷ
        /// </summary>
        public long Bigmoney
        {
            set { _bigmoney = value; }
            get { return _bigmoney; }
        }
        #endregion Model

    }
}

