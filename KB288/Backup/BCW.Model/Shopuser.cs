using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Shopuser ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Shopuser
	{
		public Shopuser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _giftid;
		private string _prevpic;
		private string _gifttitle;
		private int _total;
		private DateTime _addtime;
        private string _pic;
        /// <summary>
        /// ����ID
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
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string GiftTitle
		{
			set{ _gifttitle=value;}
			get{return _gifttitle;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int GiftId
		{
			set{ _giftid=value;}
			get{return _giftid;}
		}
		/// <summary>
		/// ����Сͼ
		/// </summary>
		public string PrevPic
		{
			set{ _prevpic=value;}
			get{return _prevpic;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
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
		/// 0Ϊ�̳�1Ϊũ��  //�۹��� 20160607
		/// </summary>
		public string PIC
        {
            set { _pic = value; }
            get { return _pic; }
        }
        #endregion Model

    }
}

