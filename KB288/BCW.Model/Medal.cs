using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Medal 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Medal
	{
		public Medal()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _imageurl;
		private string _notes;
		private int _icent;
		private int _icount;
		private int _iday;
		private string _payid;
		private string _payextime;
		private int _paixu;
		private int _forumid;
		private string _payidtemp;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 勋章类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 勋章名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 勋章图片地址
		/// </summary>
		public string ImageUrl
		{
			set{ _imageurl=value;}
			get{return _imageurl;}
		}
		/// <summary>
		/// 勋章说明
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 收费币数
		/// </summary>
		public int iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// 库存午剩余数量
		/// </summary>
		public int iCount
		{
			set{ _icount=value;}
			get{return _icount;}
		}
		/// <summary>
		/// 有效天数
		/// </summary>
		public int iDay
		{
			set{ _iday=value;}
			get{return _iday;}
		}
		/// <summary>
		/// 使用的ID（用#分开）
		/// </summary>
		public string PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// 过期时间（用#分开）
		/// </summary>
		public string PayExTime
		{
			set{ _payextime=value;}
			get{return _payextime;}
		}
		/// <summary>
		/// 后台排序
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// 论坛ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 临时ID（用#分开）
		/// </summary>
		public string PayIDtemp
		{
			set{ _payidtemp=value;}
			get{return _payidtemp;}
		}
		#endregion Model

	}
}

