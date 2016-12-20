using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类FComment 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class FComment
	{
		public FComment()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _detailid;
		private string _content;
		private int _usid;
		private string _usname;
		private string _addusip;
		private DateTime _addtime;
		private string _retext;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型（1/日志/2相册）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 主内容ID
		/// </summary>
		public int DetailId
		{
			set{ _detailid=value;}
			get{return _detailid;}
		}
		/// <summary>
		/// 评论内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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
		/// 提交IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回复内容
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		#endregion Model

	}
}

