using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类File 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class File
	{
		public File()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private string _files;
		private string _prevfiles;
		private string _content;
		private long _filesize;
		private string _fileext;
		private int _downnum;
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
		/// 附件类型（1图片/2文件）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 内容ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 文件地址
		/// </summary>
		public string Files
		{
			set{ _files=value;}
			get{return _files;}
		}
		/// <summary>
		/// 缩略图文件地址(只相对图片类型)
		/// </summary>
		public string PrevFiles
		{
			set{ _prevfiles=value;}
			get{return _prevfiles;}
		}
		/// <summary>
		/// 文件描述
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 文件大小
		/// </summary>
		public long FileSize
		{
			set{ _filesize=value;}
			get{return _filesize;}
		}
		/// <summary>
		/// 文件格式
		/// </summary>
		public string FileExt
		{
			set{ _fileext=value;}
			get{return _fileext;}
		}
		/// <summary>
		/// 下载次数
		/// </summary>
		public int DownNum
		{
			set{ _downnum=value;}
			get{return _downnum;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

