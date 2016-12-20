using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Textcent ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Textcent
	{
		public Textcent()
		{}
		#region Model
		private int _id;
		private int _bid;
		private int _usid;
		private string _usname;
		private int _toid;
		private long _cents;
		private int _bztype;
		private string _notes;
		private DateTime _addtime;
        private int _replyfloor;
        private int _paybyfund;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// �����û�ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
        /// <summary>
        /// ���ӻ����û�¥��
        /// </summary>
        public int ReplyFloor
        {
            set { _replyfloor = value; }
            get { return _replyfloor; }
        }
        /// <summary>
        /// �жϴ�������� 1Ϊ��̳���� 0Ϊ˽�˲Ʋ�
        /// </summary>
        public int PayByFund
        {
            set { _paybyfund = value; }
            get { return _paybyfund; }
        }
		/// <summary>
		/// �ͱҶ�
		/// </summary>
		public long Cents
		{
			set{ _cents=value;}
			get{return _cents;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ������ԭ��
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

