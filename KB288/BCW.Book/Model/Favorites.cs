using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����Favorites ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Favorites
	{
		public Favorites()
		{}
		#region Model
		private int _id;
        private int _favid;
		private int _types;
		private string _title;
		private int _nid;
        private int _sid;
		private string _purl;
        private int _usid;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// ���ID
        /// </summary>
        public int favid
        {
            set { _favid = value; }
            get { return _favid; }
        }
		/// <summary>
		/// ����0��ܡ�1��ǩ
		/// </summary>
		public int types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
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
        /// �鱾ID
        /// </summary>
        public int sid
        {
            set { _sid = value; }
            get { return _sid; }
        }
		/// <summary>
		/// ��ǩ��ַ
		/// </summary>
		public string purl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
        /// <summary>
        /// ��ԱID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

