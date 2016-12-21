using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Speak 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Speak
	{
		public Speak()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private string _usname;
		private int _toid;
		private string _toname;
		private string _notes;
		private int _iskiss;
		private DateTime _addtime;
		private int _istop;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 游戏类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 游戏房间ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 发言ID
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 发言昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 发给ID
		/// </summary>
		public int ToId
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 发给昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 是否秘密
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
		/// <summary>
		/// 是否置顶（0正常/1置顶）
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		#endregion Model

	}
}

