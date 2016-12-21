using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Order 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Order
	{
		public Order()
		{}
		#region Model
		private int _id;
		private int _topicid;
		private int _usid;
		private string _usname;
		private string _title;
		private int _selltypes;
		private DateTime _addtime;
		private DateTime _extime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 页面ID
		/// </summary>
		public int TopicId
		{
			set{ _topicid=value;}
			get{return _topicid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsId
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
		/// 页面标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 订单类型（0按次计费,扣费一次，永久浏览/1包周计费/2包月计费）
		/// </summary>
		public int SellTypes
		{
			set{ _selltypes=value;}
			get{return _selltypes;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 到期时间
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		#endregion Model

	}
}

