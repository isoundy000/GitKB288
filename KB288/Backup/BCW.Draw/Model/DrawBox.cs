using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类DrawBox 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DrawBox
	{
		public DrawBox()
		{}
		#region Model
		private int _id;
		private string _goodsname;
		private string _explain;
		private string _goodsimg;
		private int _goodstype;
		private int _goodsvalue;
		private int _goodsnum;
		private DateTime _addtime;
		private DateTime _overtime;
		private DateTime _receivetime;
		private int _goodscounts;
		private int _statue;
        private string _beizhu;
        private int _rank;
        private int _points;
        private int _AllNum;
        private int _lun;
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
		public string GoodsName
		{
			set{ _goodsname=value;}
			get{return _goodsname;}
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
		public string GoodsImg
		{
			set{ _goodsimg=value;}
			get{return _goodsimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsType
		{
			set{ _goodstype=value;}
			get{return _goodstype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsValue
		{
			set{ _goodsvalue=value;}
			get{return _goodsvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsNum
		{
			set{ _goodsnum=value;}
			get{return _goodsnum;}
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
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ReceiveTime
		{
			set{ _receivetime=value;}
			get{return _receivetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsCounts
		{
			set{ _goodscounts=value;}
			get{return _goodscounts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Statue
		{
			set{ _statue=value;}
			get{return _statue;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string beizhu
        {
            set { _beizhu = value; }
            get { return _beizhu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int rank
        {
            set { _rank = value; }
            get { return _rank; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        public int AllNum
        {
            set { _AllNum = value; }
            get { return _AllNum; }
        }
        /// <summary>
        /// 第几轮奖池
        /// </summary>
        public int lun
        {
            set { _lun = value; }
            get { return _lun; }
        }
		#endregion Model

	}
}

