using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类Contents 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 大分类ID
		/// </summary>
		public int shi
		{
			set{ _shi=value;}
			get{return _shi;}
		}
		/// <summary>
        /// 章节标题
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
        /// 描述
		/// </summary>
		public string summary
		{
			set{ _summary=value;}
			get{return _summary;}
		}
		/// <summary>
		/// 章节内容
		/// </summary>
		public string contents
		{
			set{ _contents=value;}
			get{return _contents;}
		}
		/// <summary>
		/// 添加时间
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
		/// 节点ID
		/// </summary>
		public int jid
		{
			set{ _jid=value;}
			get{return _jid;}
		}
		/// <summary>
		/// 搜索关键词
		/// </summary>
		public string tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
        /// <summary>
        /// 归属上传ID
        /// </summary>
        public int aid
        {
            set { _aid = value; }
            get { return _aid; }

        }
        /// <summary>
        /// 排序
        /// </summary>
        public int pid
        {
            set { _pid = value; }
            get { return _pid; }

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

