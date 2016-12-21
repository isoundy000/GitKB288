using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类ShuFav 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ShuFav
	{
		public ShuFav()
		{}
		#region Model
		private int _id;
		private string _name;
		private int _usid;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 书架名称
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 归属ID
		/// </summary>
		public int usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		#endregion Model

	}
}

