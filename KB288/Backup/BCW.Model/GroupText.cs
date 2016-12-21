using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类GroupText 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class GroupText
	{
		public GroupText()
		{}
		#region Model
		private int _id;
		private int _groupid;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private string _content;
		private int _iskiss;
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
		/// 圈子ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 发言用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 发言用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 发言给ID
		/// </summary>
		public int ToID
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 发言给昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 发言内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 是否蜜语
		/// </summary>
		public int IsKiss
		{
			set{ _iskiss=value;}
			get{return _iskiss;}
		}
		/// <summary>
		/// 发言时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

