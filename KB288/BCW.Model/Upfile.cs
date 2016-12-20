using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Upfile 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Upfile
	{
		public Upfile()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private int _forumid;
		private int _bid;
		private int _reid;
		private string _files;
		private string _prevfiles;
		private string _content;
		private long _filesize;
		private string _fileext;
		private int _downnum;
		private int _cent;
		private int _isverify;
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
		/// 类型:1相册-2音乐-3视频-4资源
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 相册文件夹ID
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
		/// 论坛ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// 帖子ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
		}
		/// <summary>
		/// 回帖ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
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
		/// 缩略图文件地址
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
		/// 收费币数
		/// </summary>
		public int Cent
		{
			set{ _cent=value;}
			get{return _cent;}
		}
		/// <summary>
		/// 是否已审核(0已审核/1未审核)
		/// </summary>
		public int IsVerify
		{
			set{ _isverify=value;}
			get{return _isverify;}
		}
		/// <summary>
		/// 上传时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

