using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类flowmyzz 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class flowmyzz
	{
		public flowmyzz()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _zid;
		private string _ztitle;
		private int _znum;
		private int _usid;
		private string _usname;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// Types(0种子，1鲜花)
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 种子ID
		/// </summary>
		public int zid
		{
			set{ _zid=value;}
			get{return _zid;}
		}
		/// <summary>
		/// 种子名称
		/// </summary>
		public string ztitle
		{
			set{ _ztitle=value;}
			get{return _ztitle;}
		}
		/// <summary>
		/// 种子数量
		/// </summary>
		public int znum
		{
			set{ _znum=value;}
			get{return _znum;}
		}
		/// <summary>
		/// 会员ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 会员昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		#endregion Model

	}
}

