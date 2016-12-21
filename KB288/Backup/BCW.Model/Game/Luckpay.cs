using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Luckpay ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Luckpay
	{
		public Luckpay()
		{}
		#region Model
		private int _id;
		private int _luckid;
		private int _usid;
		private string _usname;
		private string _buynum;
		private long _buycent;
        private long _buycents;
		private long _wincent;
		private int _state;
        private string _buytype;
		private DateTime _addtime;
        private int _isrobot;
        private string _odds;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
		/// ��ע����
		/// </summary>
		public string odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
        /// <summary>
        /// �Ƿ��е����ע 0���ǣ� 1��
        /// </summary>
        public int IsRobot
        {
            set { _isrobot = value; }
            get { return _isrobot; }
        }
		/// <summary>
		/// ���˼�¼ID
		/// </summary>
		public int LuckId
		{
			set{ _luckid=value;}
			get{return _luckid;}
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
        /// ���������
        /// </summary>
        public string BuyType
        {
            set { _buytype = value; }
            get { return _buytype; }
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
		/// ������������֣����ŷֿ���
		/// </summary>
		public string BuyNum
		{
			set{ _buynum=value;}
			get{return _buynum;}
		}
		/// <summary>
		/// ��ע����
		/// </summary>
		public long BuyCent
		{
			set{ _buycent=value;}
			get{return _buycent;}
		}
        /// <summary>
        /// ��ע�ܱ���
        /// </summary>
        public long BuyCents
        {
            set { _buycents = value; }
            get { return _buycents; }
        }
		/// <summary>
		/// Ӯ����
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// ״̬��0δ���/1�����/2�Ѷҽ���
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ��עʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

