using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类GiftFlows 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class GiftFlows
	{
		public GiftFlows()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private int _total;
		private int _totall;
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
		/// 总数量
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 剩余数量
		/// </summary>
		public int Totall
		{
			set{ _totall=value;}
			get{return _totall;}
		}
		#endregion Model

	}
}

