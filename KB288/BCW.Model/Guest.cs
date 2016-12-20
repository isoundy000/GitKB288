using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Guest 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 类型
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
		/// <summary>
		/// 发送ID
		/// </summary>
		public int FromId
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// 发送昵称
		/// </summary>
		public string FromName
		{
			set{ _fromname=value;}
			get{return _fromname;}
		}
		/// <summary>
		/// 接收ID
		/// </summary>
		public int ToId
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 接收昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 消息内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 是否已读
		/// </summary>
		public int IsSeen
		{
			set{ _isseen=value;}
			get{return _isseen;}
		}
		/// <summary>
		/// 是否收藏
		/// </summary>
		public int IsKeep
		{
			set{ _iskeep=value;}
			get{return _iskeep;}
		}
		/// <summary>
		/// 发送方是否删除
		/// </summary>
		public int FDel
		{
			set{ _fdel=value;}
			get{return _fdel;}
		}
		/// <summary>
		/// 接收方是否删除
		/// </summary>
		public int TDel
		{
			set{ _tdel=value;}
			get{return _tdel;}
		}
		/// <summary>
		/// 转自ID
		/// </summary>
		public int TransId
		{
			set{ _transid=value;}
			get{return _transid;}
		}
		/// <summary>
		/// 发送IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

