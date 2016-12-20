using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类klsfpay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class klsfpay
	{
		public klsfpay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _klsfid;
		private int _usid;
		private string _usname;
		private long _price;
		private int _icount;
		private string _notes;
		private string _result;
		private long _prices;
		private long _wincent;
		private int _state;
		private DateTime _addtime;
        private int _iwin;
        private int _isRoBot;
        private decimal _odds;

        /// <summary>
        /// 
        /// </summary>
        public int iWin
        {
            get { return _iwin; }
            set { _iwin = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int klsfId
		{
			set{ _klsfid=value;}
			get{return _klsfid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int iCount
		{
			set{ _icount=value;}
			get{return _icount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long Prices
		{
			set{ _prices=value;}
			get{return _prices;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int isRoBot
        {
            set { _isRoBot = value; }
            get { return _isRoBot; }
        }
        /// <summary>
        /// 赔率
        /// </summary>
        public decimal Odds
        {
            set { _odds = value; }
            get { return _odds; }
        }
		#endregion Model

	}
}

