using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Forumstat 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Forumstat
	{
		public Forumstat()
		{}
		#region Model
		private int _id;
		private int _forumid;
		private int _usid;
		private string _usname;
		private int _ttotal;
		private int _rtotal;
		private int _gtotal;
		private int _jtotal;
		private int _hjtotal;
		private int _zttotal;
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
		/// 论坛ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
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
		/// 发帖数量
		/// </summary>
		public int tTotal
		{
			set{ _ttotal=value;}
			get{return _ttotal;}
		}
		/// <summary>
		/// 回帖数量
		/// </summary>
		public int rTotal
		{
			set{ _rtotal=value;}
			get{return _rtotal;}
		}
		/// <summary>
		/// 帖子被加精数量
		/// </summary>
		public int gTotal
		{
			set{ _gtotal=value;}
			get{return _gtotal;}
		}
		/// <summary>
		/// 帖子被推荐数量
		/// </summary>
		public int jTotal
		{
			set{ _jtotal=value;}
			get{return _jtotal;}
		}
		/// <summary>
		/// 回帖加精数量
		/// </summary>
		public int hjTotal
		{
			set{ _hjtotal=value;}
			get{return _hjtotal;}
		}
		/// <summary>
		/// 加专题数量
		/// </summary>
		public int ztTotal
		{
			set{ _zttotal=value;}
			get{return _zttotal;}
		}
		/// <summary>
		/// 更新日期
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

