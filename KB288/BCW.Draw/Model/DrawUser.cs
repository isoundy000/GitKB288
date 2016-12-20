using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类DrawUser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DrawUser
	{
		public DrawUser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _GoodsCounts;
		private string _mygoods;
		private string _explain;
		private string _mygoodsimg;
		private int _mygoodstype;
		private int _mygoodsvalue;
		private int _mygoodsstatue;
		private int _mygoodsnum;
		private DateTime _ontime;
		private DateTime _intime;
		private string _address;
		private string _phone;
		private string _email;
        private int _R;
        private int _Num;
        private string _RealName;
        private string _Express;
        private string _Numbers;
        private int _aa;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
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
		public int GoodsCounts
		{
			set{ _GoodsCounts=value;}
			get{return _GoodsCounts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MyGoods
		{
			set{ _mygoods=value;}
			get{return _mygoods;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Explain
		{
			set{ _explain=value;}
			get{return _explain;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MyGoodsImg
		{
			set{ _mygoodsimg=value;}
			get{return _mygoodsimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MyGoodsType
		{
			set{ _mygoodstype=value;}
			get{return _mygoodstype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MyGoodsValue
		{
			set{ _mygoodsvalue=value;}
			get{return _mygoodsvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MyGoodsStatue
		{
			set{ _mygoodsstatue=value;}
			get{return _mygoodsstatue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MyGoodsNum
		{
			set{ _mygoodsnum=value;}
			get{return _mygoodsnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OnTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime InTime
		{
			set{ _intime=value;}
			get{return _intime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int R
        {
            set { _R = value; }
            get { return _R; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Num
        {
            set { _Num = value; }
            get { return _Num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            set { _RealName = value; }
            get { return _RealName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Express
        {
            set { _Express = value; }
            get { return _Express; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Numbers
        {
            set { _Numbers = value; }
            get { return _Numbers; }
        }
		#endregion Model

	}
}

