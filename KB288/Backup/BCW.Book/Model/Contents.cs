using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����Contents ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Contents
	{
		public Contents()
		{}
		#region Model
		private int _id;
		private int _shi;
		private string _title;
		private string _summary;
		private string _contents;
		private DateTime _addtime;
		private int _state;
		private int _jid;
		private string _tags;
        private int _aid;
        private int _pid;
        private int _isdel;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �����ID
		/// </summary>
		public int shi
		{
			set{ _shi=value;}
			get{return _shi;}
		}
		/// <summary>
        /// �½ڱ���
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
        /// ����
		/// </summary>
		public string summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
		/// �½�����
		/// </summary>
		public string contents
		{
			set{ _contents=value;}
			get{return _contents;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
        /// ״̬ 1��� 0δ���
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// �ڵ�ID
		/// </summary>
		public int jid
		{
			set{ _jid=value;}
			get{return _jid;}
		}
		/// <summary>
		/// �����ؼ���
		/// </summary>
		public string tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
        /// <summary>
        /// �����ϴ�ID
        /// </summary>
        public int aid
        {
            set { _aid = value; }
            get { return _aid; }

        }
        /// <summary>
        /// ����
        /// </summary>
        public int pid
        {
            set { _pid = value; }
            get { return _pid; }

        }
        /// <summary>
        /// �Ƿ���ɾ��(0����/1ǰ̨��ɾ��)
        /// </summary>
        public int isdel
        {
            set { _isdel = value; }
            get { return _isdel; }
        }
		#endregion Model

	}
}

