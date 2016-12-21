using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Textcent 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 帖子ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 帖子用户ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
        /// <summary>
        /// 帖子回帖用户楼层
        /// </summary>
        public int ReplyFloor
        {
            set { _replyfloor = value; }
            get { return _replyfloor; }
        }
        /// <summary>
        /// 判断从哪里打赏 1为论坛基金 0为私人财产
        /// </summary>
        public int PayByFund
        {
            set { _paybyfund = value; }
            get { return _paybyfund; }
        }
		/// <summary>
		/// 赏币额
		/// </summary>
		public long Cents
		{
			set{ _cents=value;}
			get{return _cents;}
		}
		/// <summary>
		/// 币种
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 描述与原因
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

