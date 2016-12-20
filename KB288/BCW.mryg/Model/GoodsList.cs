using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类GoodsList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class GoodsList
	{
		public GoodsList()
		{}
		#region Model
		private long _id;
		private string _goodsname;
		private string _explain;
		private string _goodsimg;
		private int? _imgcounts;
		private long _goodsvalue;
		private long _periods;
		private long _number;
		private DateTime? _addtime;
		private DateTime? _overtime;
		private int? _goodstype;
		private DateTime? _remainingtime;
		private string _winner;
		private DateTime? _lotterytime;
		private int? _statue;
		private int? _isdone;
        private string _goodsfrom;
        private int? _identification;
        private string _stockyungouma;
        private int? _goodssell;
		/// <summary>
		/// 
		/// </summary>
		public long Id
		{
			set{ _id=value;}
			get{return _id;}
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
		public string explain
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
		public int? ImgCounts
		{
			set{ _imgcounts=value;}
			get{return _imgcounts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long GoodsValue
		{
			set{ _goodsvalue=value;}
			get{return _goodsvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long periods
		{
			set{ _periods=value;}
			get{return _periods;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long Number
		{
			set{ _number=value;}
			get{return _number;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GoodsType
		{
			set{ _goodstype=value;}
			get{return _goodstype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? RemainingTime
		{
			set{ _remainingtime=value;}
			get{return _remainingtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Winner
		{
			set{ _winner=value;}
			get{return _winner;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? lotteryTime
		{
			set{ _lotterytime=value;}
			get{return _lotterytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? statue
		{
			set{ _statue=value;}
			get{return _statue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string GoodsFrom
        {
            set { _goodsfrom = value; }
            get { return _goodsfrom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Identification
        {
            set { _identification = value; }
            get { return _identification; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StockYungouma
        {
            set { _stockyungouma = value; }
            get { return _stockyungouma; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GoodsSell
        {
            set { _goodssell = value; }
            get { return _goodssell; }
        }
		#endregion Model

	}
}

