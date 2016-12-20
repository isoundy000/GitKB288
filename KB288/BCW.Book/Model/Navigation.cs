using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类Navigation 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Navigation
	{
		public Navigation()
		{}
		#region Model
		private int _id;
		private int _pid;
		private string _name;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 排序ID
		/// </summary>
		public int pid
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// 分类名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		#endregion Model

	}
}

