using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类DrawDS 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class DrawDS
	{
		public DrawDS()
		{}
		#region Model
		private int _id;
		private int? _goodscounts;
		private string _gamename;
		private string _ds;
		private int? _dsid;
		private string _one;
		private string _two;
		private string _three;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 游戏名
		/// </summary>
		public string gamename
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// 道具或者属性名
		/// </summary>
		public string DS
		{
			set{ _ds=value;}
			get{return _ds;}
		}
		/// <summary>
		/// 道具或者属性的ID
		/// </summary>
		public int? DSID
		{
			set{ _dsid=value;}
			get{return _dsid;}
		}
		/// <summary>
		/// 奖品数量
		/// </summary>
		public string one
		{
			set{ _one=value;}
			get{return _one;}
		}
		/// <summary>
		/// 抽中时间
		/// </summary>
		public string two
		{
			set{ _two=value;}
			get{return _two;}
		}
		/// <summary>
		/// 奖品状态
		/// </summary>
		public string three
		{
			set{ _three=value;}
			get{return _three;}
		}
		#endregion Model

	}
}

