using System;
namespace BCW.SSE.Model
{

    public class veSseGetPrize
    {
        private int _id;
        private int _orderId;
        private bool _isGet;
        private int _orderType;
        private int _sseNo;
        private bool _buyType;
        private decimal _buyMoney;
        private decimal _prizeVal;

	    public int id
	    {
		    set{ _id=value;}
		    get{return _id;}
	    }

        public int orderId
        {
            set
            {
                _orderId = value;
            }
            get
            {
                return _orderId;
            }
        }

        public bool isGet
        {
            set
            {
                _isGet = value;
            }
            get
            {
                return _isGet;
            }
        }

        public int orderType
        {
            set
            {
                _orderType = value;
            }
            get
            {
                return _orderType;
            }
        }

        public int sseNo
        {
            set
            {
                _sseNo = value;
            }
            get
            {
                return _sseNo;
            }
        }

        public bool buyType
        {
            set
            {
                _buyType = value;
            }
            get
            {
                return _buyType;
            }
        }

        public decimal buyMoney
        {
            set
            {
                _buyMoney = value;
            }
            get
            {
                return _buyMoney;
            }
        }

        public decimal prizeVal
        {
            set
            {
                _prizeVal = value;
            }
            get
            {
                return _prizeVal;
            }
        }
       
    }

    public class SseGetPrizeCharts
    {
        private int _userId;
        private Int64 _prizeVal;

        public int userId
        {
            set
            {
                _userId = value;
            }
            get
            {
                return _userId;
            }
        }


        public Int64 prizeVal
        {
            set
            {
                _prizeVal = value;
            }
            get
            {
                return _prizeVal;
            }
        }

    }


	/// <summary>
	/// SseGetPrize:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SseGetPrize
	{
		public SseGetPrize()
		{}
		#region Model
		private int _id;
		private int _orderid;
		private int _userid;
		private bool _isget= false;
		private DateTime _getdatetime;
        private DateTime _opendatetime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int orderId
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int userId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool isGet
		{
			set{ _isget=value;}
			get{return _isget;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime getDateTime
		{
			set{ _getdatetime=value;}
			get{return _getdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
        public DateTime openDateTime
		{
			set{ _opendatetime=value;}
			get{return _opendatetime;}
		}
		#endregion Model

	}
}

