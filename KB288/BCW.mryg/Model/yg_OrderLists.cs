using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类yg_OrderLists 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class yg_OrderLists
	{
		public yg_OrderLists()
		{}
		#region Model
		private int _id;
		private long _goodslistid;
		private long _buylistid;
		private DateTime? _addtime;
		private DateTime? _posttime;
		private DateTime? _overtime;
		private string _wuliu;
		private string _yundanhao;
		private string _operator;
		private string _operatornotes;
		private int? _operatorstatue;
		private int? _wuliustatue;
		private string _consignee;
		private string _address;
		private int? _zipcode;
		private string _phonemobile;
		private string _phonehome;
		private string _consigneenotes;
		private string _consigneestatue;
		private int? _statue;
		private int? _isdone;
		private string _spare;
		private string _brack;
		private int? _userid;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long GoodsListId
		{
			set{ _goodslistid=value;}
			get{return _goodslistid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long BuyListId
		{
			set{ _buylistid=value;}
			get{return _buylistid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? PostTime
		{
			set{ _posttime=value;}
			get{return _posttime;}
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
		public string wuliu
		{
			set{ _wuliu=value;}
			get{return _wuliu;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string yundanhao
		{
			set{ _yundanhao=value;}
			get{return _yundanhao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Operator
		{
			set{ _operator=value;}
			get{return _operator;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OperatorNotes
		{
			set{ _operatornotes=value;}
			get{return _operatornotes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OperatorStatue
		{
			set{ _operatorstatue=value;}
			get{return _operatorstatue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? wuliuStatue
		{
			set{ _wuliustatue=value;}
			get{return _wuliustatue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Consignee
		{
			set{ _consignee=value;}
			get{return _consignee;}
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
		public int? ZipCode
		{
			set{ _zipcode=value;}
			get{return _zipcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PhoneMobile
		{
			set{ _phonemobile=value;}
			get{return _phonemobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PhoneHome
		{
			set{ _phonehome=value;}
			get{return _phonehome;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConsigneeNotes
		{
			set{ _consigneenotes=value;}
			get{return _consigneenotes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConsigneeStatue
		{
			set{ _consigneestatue=value;}
			get{return _consigneestatue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Statue
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
		public string Spare
		{
			set{ _spare=value;}
			get{return _spare;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string brack
		{
			set{ _brack=value;}
			get{return _brack;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		#endregion Model

	}
}

