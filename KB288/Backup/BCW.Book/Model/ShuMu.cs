using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类ShuMu 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 书本名称
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
        /// 书本简介
		/// </summary>
		public string summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
        /// 书本图片
		/// </summary>
		public string img
		{
			set{ _img=value;}
			get{return _img;}
		}
		/// <summary>
        /// 上传id
		/// </summary>
		public int aid
		{
			set{ _aid=value;}
			get{return _aid;}
		}
		/// <summary>
        /// 作者名称
		/// </summary>
		public string author
		{
			set{ _author=value;}
			get{return _author;}
		}
		/// <summary>
        /// 发布日期
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
        /// 状态 1审核 0未审核
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 关键词
		/// </summary>
		public string tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
        /// <summary>
        /// 0原创 1转载
        /// </summary>
        public int types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 书本字数(0表示未定)
        /// </summary>
        public int length
        {
            set { _length = value; }
            get { return _length; }
        }
        /// <summary>
        /// 书本人气
        /// </summary>
        public int click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// 书本推荐数
        /// </summary>
        public int good
        {
            set { _good = value; }
            get { return _good; }
        }
        /// <summary>
        /// 最后更新
        /// </summary>
        public DateTime gxtime
        {
            set { _gxtime = value; }
            get { return _gxtime; }
        }
        /// <summary>
        /// 0连载中 1已完结
        /// </summary>
        public int isover
        {
            set { _isover = value; }
            get { return _isover; }
        }
        /// <summary>
        /// 0普通 1精品
        /// </summary>
        public int isgood
        {
            set { _isgood = value; }
            get { return _isgood; }
        }
        /// <summary>
        /// 书评数目
        /// </summary>
        public int pl
        {
            set { _pl = value; }
            get { return _pl; }
        }
        /// <summary>
        /// 更新提醒ID
        /// </summary>
        public string gxids
        {
            set { _gxids = value; }
            get { return _gxids; }
        }
        /// <summary>
        /// 是否已删除(0正常/1前台已删除)
        /// </summary>
        public int isdel
        {
            set { _isdel = value; }
            get { return _isdel; }
        }
		#endregion Model

	}
}

