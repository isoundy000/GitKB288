using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����ShuMu ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ShuMu
	{
		public ShuMu()
		{}
		#region Model
		private int _id;
		private int _nid;
		private string _title;
		private string _summary;
		private string _img;
		private int _aid;
		private string _author;
		private DateTime _addtime;
		private int _state;
		private string _tags;
        private int _types;
        private int _length;
        private int _click;
        private int _good;
        private DateTime _gxtime;
        private int _isover;
        private int _isgood;
        private int _pl;
        private string _gxids;
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
		/// ����ID
		/// </summary>
		public int nid
		{
			set{ _nid=value;}
			get{return _nid;}
		}
		/// <summary>
		/// �鱾����
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
        /// �鱾���
		/// </summary>
		public string summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
        /// �鱾ͼƬ
		/// </summary>
		public string img
		{
			set{ _img=value;}
			get{return _img;}
		}
		/// <summary>
        /// �ϴ�id
		/// </summary>
		public int aid
		{
			set{ _aid=value;}
			get{return _aid;}
		}
		/// <summary>
        /// ��������
		/// </summary>
		public string author
		{
			set{ _author=value;}
			get{return _author;}
		}
		/// <summary>
        /// ��������
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
		/// �ؼ���
		/// </summary>
		public string tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
        /// <summary>
        /// 0ԭ�� 1ת��
        /// </summary>
        public int types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// �鱾����(0��ʾδ��)
        /// </summary>
        public int length
        {
            set { _length = value; }
            get { return _length; }
        }
        /// <summary>
        /// �鱾����
        /// </summary>
        public int click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// �鱾�Ƽ���
        /// </summary>
        public int good
        {
            set { _good = value; }
            get { return _good; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public DateTime gxtime
        {
            set { _gxtime = value; }
            get { return _gxtime; }
        }
        /// <summary>
        /// 0������ 1�����
        /// </summary>
        public int isover
        {
            set { _isover = value; }
            get { return _isover; }
        }
        /// <summary>
        /// 0��ͨ 1��Ʒ
        /// </summary>
        public int isgood
        {
            set { _isgood = value; }
            get { return _isgood; }
        }
        /// <summary>
        /// ������Ŀ
        /// </summary>
        public int pl
        {
            set { _pl = value; }
            get { return _pl; }
        }
        /// <summary>
        /// ��������ID
        /// </summary>
        public string gxids
        {
            set { _gxids = value; }
            get { return _gxids; }
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

