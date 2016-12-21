using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Race 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Race
	{
		public Race()
		{}
		#region Model
		private int _id;
		private int _userid;
		private string _username;
		private string _title;
		private string _fileurl;
		private string _content;
		private string _pcontent;
		private long _price;
		private DateTime _writetime;
		private DateTime _totime;
		private int _paycount;
		private int _paytype;
		private long _topprice;
		private int _winid;
		private string _winname;
		private int _iscase;
		private string _notes;
		private int _types;
		private DateTime _writedate;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int userid
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string username
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string fileurl
		{
			set{ _fileurl=value;}
			get{return _fileurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pcontent
		{
			set{ _pcontent=value;}
			get{return _pcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime writetime
		{
			set{ _writetime=value;}
			get{return _writetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime totime
		{
			set{ _totime=value;}
			get{return _totime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int payCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int paytype
		{
			set{ _paytype=value;}
			get{return _paytype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long topPrice
		{
			set{ _topprice=value;}
			get{return _topprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int winID
		{
			set{ _winid=value;}
			get{return _winid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string winName
		{
			set{ _winname=value;}
			get{return _winname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int isCase
		{
			set{ _iscase=value;}
			get{return _iscase;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime writedate
		{
			set{ _writedate=value;}
			get{return _writedate;}
		}
		#endregion Model

	}
}

