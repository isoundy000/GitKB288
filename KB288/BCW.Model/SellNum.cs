using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类SellNum 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SellNum
	{
		public SellNum()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _buyuid;
		private long _price;
		private int _state;
		private DateTime _addtime;
		private string _mobile;
		private string _notes;
		private int _tags;
		private DateTime _paytime;
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
			set{ _types=value;}
			get{return _types;}
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
		/// 购买的ID
		/// </summary>
		public int BuyUID
		{
			set{ _buyuid=value;}
			get{return _buyuid;}
		}
		/// <summary>
		/// ID价格
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 状态(1查询中/2已报价/3已申请兑换/4已成功)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 绑定的手机号
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 系统备注
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// QQ服务类型
		/// </summary>
		public int Tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
		/// <summary>
		/// 成交时间
		/// </summary>
		public DateTime PayTime
		{
			set{ _paytime=value;}
			get{return _paytime;}
		}
		#endregion Model

	}
}

