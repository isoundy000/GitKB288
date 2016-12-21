using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Shopsend 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Shopsend
	{
		public Shopsend()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _giftid;
		private string _prevpic;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private int _total;
		private string _message;
		private DateTime _addtime;
        private string _pic;
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 礼物名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 礼物ID
		/// </summary>
		public int GiftId
		{
			set{ _giftid=value;}
			get{return _giftid;}
		}
		/// <summary>
		/// 礼物小图
		/// </summary>
		public string PrevPic
		{
			set{ _prevpic=value;}
			get{return _prevpic;}
		}
		/// <summary>
		/// 送自ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 送自昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 送到ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 送到昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 收到礼物数量
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 赠送附言
		/// </summary>
		public string Message
		{
			set{ _message=value;}
			get{return _message;}
		}
		/// <summary>
		/// 赠送时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
        /// 区分农场或商城  0商城1农场  //邵广林 20160606 农场
        /// </summary>
        public string PIC
        {
            set { _pic = value; }
            get { return _pic; }
        }
        #endregion Model

    }
}

