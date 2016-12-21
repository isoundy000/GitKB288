using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类ShuBbs 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ShuBbs
	{
		public ShuBbs()
		{}
		#region Model
		private int _id;
		private int _nid;
		private int _usid;
		private string _usname;
		private string _content;
		private DateTime _addtime;
		private string _addusip;
		private string _retext;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 书本ID
		/// </summary>
		public int nid
		{
			set{ _nid=value;}
			get{return _nid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string usname
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 评论内容
		/// </summary>
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 提交IP
		/// </summary>
		public string addusip
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 回复内容（备用）
		/// </summary>
		public string retext
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		#endregion Model

	}
}

