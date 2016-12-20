using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类Favorites 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 书架ID
        /// </summary>
        public int favid
        {
            set { _favid = value; }
            get { return _favid; }
        }
		/// <summary>
		/// 类型0书架、1书签
		/// </summary>
		public int types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 分类ID
		/// </summary>
		public int nid
		{
			set{ _nid=value;}
			get{return _nid;}
		}
        /// <summary>
        /// 书本ID
        /// </summary>
        public int sid
        {
            set { _sid = value; }
            get { return _sid; }
        }
		/// <summary>
		/// 书签地址
		/// </summary>
		public string purl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
        /// <summary>
        /// 会员ID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

