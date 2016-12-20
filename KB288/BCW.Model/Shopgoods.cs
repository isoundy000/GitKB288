using System;
namespace BCW.Shop.Model
{
	/// <summary>
	/// ʵ����Shopgoods ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Shopgoods
	{
		public Shopgoods()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _giftid;
		private string _prevpic;
		private int _num;
		private DateTime _buytime;
		private DateTime? _sendtime;
		private DateTime? _receivetime;
		private int _usid;
		private string _address;
		private string _phone;
		private string _email;
		private string _realname;
		private string _message;
        private int _ShopGiftId;
        private string _Express;
        private string _Expressnum;

		/// <summary>
		/// ��ʶID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��ƷId
		/// </summary>
		public int GiftId
		{
			set{ _giftid=value;}
			get{return _giftid;}
		}
		/// <summary>
		/// ͼƬ
		/// </summary>
		public string PrevPic
		{
			set{ _prevpic=value;}
			get{return _prevpic;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public int Num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime BuyTime
		{
			set{ _buytime=value;}
			get{return _buytime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime? SendTime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// �ջ�ʱ��
		/// </summary>
		public DateTime? ReceiveTime
		{
			set{ _receivetime=value;}
			get{return _receivetime;}
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
		/// �ջ���ַ
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// ��ϵ��ʽ
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string RealName
		{
			set{ _realname=value;}
			get{return _realname;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
            	/// <summary>
		/// ��Ʒid
		/// </summary>
        public int ShopGiftId
		{
            set { _ShopGiftId = value; }
            get { return _ShopGiftId; }
		}
        /// <summary>
        /// ��ݹ�˾
        /// </summary>
        public string Express
        {
            set { _Express = value; }
            get { return _Express; }
        }
        /// <summary>
        /// ��ݵ���
        /// </summary>
        public string Expressnum
        {
            set { _Expressnum = value; }
            get { return _Expressnum; }
        }
		#endregion Model

	}
}

