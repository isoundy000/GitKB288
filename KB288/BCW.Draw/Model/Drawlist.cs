using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类Drawlist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Drawlist
	{
		public Drawlist()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private DateTime? _time;
		private int? _type;
		private int? _goodscounts;
		private int? _jifen;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int? UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime? Time
		{
			set{ _time=value;}
			get{return _time;}
		}
		/// <summary>
		/// 方式类型
		/// </summary>
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 奖品编号
		/// </summary>
		public int? GoodsCounts
		{
			set{ _goodscounts=value;}
			get{return _goodscounts;}
		}
		/// <summary>
		/// 积分
		/// </summary>
		public int? Jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
		#endregion Model

	}
}

