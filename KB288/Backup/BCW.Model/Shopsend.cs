using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Shopsend ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Shopsend
	{
		public Shopsend()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _giftid;
		private string _prevpic;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private int _total;
		private string _message;
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
		/// ��������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
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
		/// ����ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// �͵�ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// �͵��ǳ�
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// �յ���������
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// ���͸���
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
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
        /// ����ũ�����̳�  0�̳�1ũ��  //�۹��� 20160606 ũ��
        /// </summary>
        public string PIC
        {
            set { _pic = value; }
            get { return _pic; }
        }
        #endregion Model

    }
}

