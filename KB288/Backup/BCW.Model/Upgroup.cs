using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Upgroup 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Upgroup
	{
		public Upgroup()
		{}
		#region Model
		private int _id;
		private int _leibie;
		private int _types;
		private int _posttype;
		private string _title;
		private string _node;
		private int _usid;
		private int _isreview;
		private int _paixu;
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
		/// 类型（0日记/1相册/2音乐/3视频/4资源）
		/// </summary>
		public int Leibie
		{
			set{ _leibie=value;}
			get{return _leibie;}
		}
		/// <summary>
		/// 分组性质（0公开/1好友可见/2私有）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 相集类型
		/// </summary>
		public int PostType
		{
			set{ _posttype=value;}
			get{return _posttype;}
		}
		/// <summary>
		/// 分组名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 封面或备注
		/// </summary>
		public string Node
		{
			set{ _node=value;}
			get{return _node;}
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
		/// 用户ID
		/// </summary>
		public int IsReview
		{
			set{ _isreview=value;}
			get{return _isreview;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

