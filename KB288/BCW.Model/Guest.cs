using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Guest ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Guest
	{
		public Guest()
		{}
		#region Model
		private int _id;
        private int _types;
		private int _fromid;
		private string _fromname;
		private int _toid;
		private string _toname;
		private string _content;
		private int _isseen;
		private int _iskeep;
		private int _fdel;
		private int _tdel;
		private int _transid;
		private string _addusip;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// ����
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
		/// <summary>
		/// ����ID
		/// </summary>
		public int FromId
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string FromName
		{
			set{ _fromname=value;}
			get{return _fromname;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int ToId
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// ��Ϣ����
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// �Ƿ��Ѷ�
		/// </summary>
		public int IsSeen
		{
			set{ _isseen=value;}
			get{return _isseen;}
		}
		/// <summary>
		/// �Ƿ��ղ�
		/// </summary>
		public int IsKeep
		{
			set{ _iskeep=value;}
			get{return _iskeep;}
		}
		/// <summary>
		/// ���ͷ��Ƿ�ɾ��
		/// </summary>
		public int FDel
		{
			set{ _fdel=value;}
			get{return _fdel;}
		}
		/// <summary>
		/// ���շ��Ƿ�ɾ��
		/// </summary>
		public int TDel
		{
			set{ _tdel=value;}
			get{return _tdel;}
		}
		/// <summary>
		/// ת��ID
		/// </summary>
		public int TransId
		{
			set{ _transid=value;}
			get{return _transid;}
		}
		/// <summary>
		/// ����IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
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

