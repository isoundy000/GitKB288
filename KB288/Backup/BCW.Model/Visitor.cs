using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Visitor 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Visitor
	{
		public Visitor()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _visitid;
		private string _visitname;
		private DateTime _addtime;
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
		/// 来访ID
		/// </summary>
		public int VisitId
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// 来访ID昵称
		/// </summary>
		public string VisitName
		{
			set{ _visitname=value;}
			get{return _visitname;}
		}
		/// <summary>
		/// 来访时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

