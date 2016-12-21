using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类xitonghao 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class xitonghao
	{
		public xitonghao()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _ip;
		private int _type;
		private DateTime? _addtime;
		private int _caoid;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 0增加1删除
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
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
		/// 被操作的ID
		/// </summary>
		public int caoID
		{
			set{ _caoid=value;}
			get{return _caoid;}
		}
		#endregion Model

	}
}

