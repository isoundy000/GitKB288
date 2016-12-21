using System;
namespace BCW.Model.Collec
{
	/// <summary>
	/// 实体类Collecdata 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Collecdata
	{
		public Collecdata()
		{}
		#region Model
		private int _id;
		private int _itemid;
		private int _types;
		private int _nodeid;
		private string _title;
		private string _keyword;
		private string _content;
		private string _pics;
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
		/// 采集项目ID
		/// </summary>
		public int ItemId
		{
			set{ _itemid=value;}
			get{return _itemid;}
		}
		/// <summary>
		/// 采集类型（1文章/2图片）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 采集栏目ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 关键字
		/// </summary>
		public string KeyWord
		{
			set{ _keyword=value;}
			get{return _keyword;}
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 图片地址
		/// </summary>
		public string Pics
		{
			set{ _pics=value;}
			get{return _pics;}
		}
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

