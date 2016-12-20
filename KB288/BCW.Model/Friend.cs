using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Friend 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Friend
	{
		public Friend()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private string _usname;
		private int _friendid;
		private string _friendname;
		private DateTime? _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类别（0/好友/1/黑名单/2/关注）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 分组ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
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
		/// 好友ID
		/// </summary>
		public int FriendID
		{
			set{ _friendid=value;}
			get{return _friendid;}
		}
		/// <summary>
		/// 好友昵称
		/// </summary>
		public string FriendName
		{
			set{ _friendname=value;}
			get{return _friendname;}
		}
		/// <summary>
		/// 最近联系时间
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

