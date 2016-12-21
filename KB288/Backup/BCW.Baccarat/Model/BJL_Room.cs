using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// 实体类BJL_Room 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BJL_Room
	{
		public BJL_Room()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _total;
		private long _lowtotal;
		private string _title;
		private string _contact;
		private DateTime _addtime;
		private int _state;
		private long _zhui_total;
        private int _type;
        private long _total_now;
        private long _shouxufei;
        private int _click;
        private long _bigpay;
        private long _bigmoney;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 彩池
		/// </summary>
		public long Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 最低彩池
		/// </summary>
		public long LowTotal
		{
			set{ _lowtotal=value;}
			get{return _lowtotal;}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string contact
		{
			set{ _contact=value;}
			get{return _contact;}
		}
		/// <summary>
		/// 开房时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 0开庄1封装
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 追加
		/// </summary>
		public long zhui_Total
		{
			set{ _zhui_total=value;}
			get{return _zhui_total;}
		}
        /// <summary>
        /// 0未发牌1准备发牌
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 实时彩池
        /// </summary>
        public long Total_Now
        {
            set { _total_now = value; }
            get { return _total_now; }
        }
        /// <summary>
        /// 手续费
        /// </summary>
        public long shouxufei
        {
            set { _shouxufei = value; }
            get { return _shouxufei; }
        }
        /// <summary>
        /// 人气
        /// </summary>
        public int Click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// 最高彩池
        /// </summary>
        public long BigPay
        {
            set { _bigpay = value; }
            get { return _bigpay; }
        }
        /// <summary>
        /// 每局每ID最大限投
        /// </summary>
        public long Bigmoney
        {
            set { _bigmoney = value; }
            get { return _bigmoney; }
        }
        #endregion Model

    }
}

