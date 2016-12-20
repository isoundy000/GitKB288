using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类MarryBook 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class MarryBook
	{
		public MarryBook()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private int _reid;
		private string _rename;
		private string _content;
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
		/// 结婚ID
		/// </summary>
		public int MarryId
		{
			set{ _marryid=value;}
			get{return _marryid;}
		}
		/// <summary>
		/// 留言ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// 留言昵称
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// 留言内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 揭底时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

