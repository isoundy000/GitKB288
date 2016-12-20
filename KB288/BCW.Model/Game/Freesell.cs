using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Freesell 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Freesell
	{
		public Freesell()
		{}
		#region Model
		private int _id;
		private int _cclid;
		private int _userid;
		private string _username;
		private string _title;
		private string _content;
		private decimal _odds;
		private int _price;
		private int _counts;
		private int _counts2;
		private DateTime _closetime;
		private DateTime _opentime;
		private DateTime _opentime2;
		private int _state;
		private int _openstats;
		private int _isgood;
		private string _opentext;
		private string _openbbs;
		private DateTime _openbbstime;
		private int _ccluserid;
		private string _cclusername;
		private int _cclitype;
		private string _ccliname;
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
		public int cclID
		{
			set{ _cclid=value;}
			get{return _cclid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Odds
		{
			set{ _odds=value;}
			get{return _odds;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Counts
		{
			set{ _counts=value;}
			get{return _counts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Counts2
		{
			set{ _counts2=value;}
			get{return _counts2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CloseTime
		{
			set{ _closetime=value;}
			get{return _closetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OpenTime
		{
			set{ _opentime=value;}
			get{return _opentime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OpenTime2
		{
			set{ _opentime2=value;}
			get{return _opentime2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OpenStats
		{
			set{ _openstats=value;}
			get{return _openstats;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsGood
		{
			set{ _isgood=value;}
			get{return _isgood;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OpenText
		{
			set{ _opentext=value;}
			get{return _opentext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Openbbs
		{
			set{ _openbbs=value;}
			get{return _openbbs;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OpenbbsTime
		{
			set{ _openbbstime=value;}
			get{return _openbbstime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int cclUserID
		{
			set{ _ccluserid=value;}
			get{return _ccluserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cclUserName
		{
			set{ _cclusername=value;}
			get{return _cclusername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ccliType
		{
			set{ _cclitype=value;}
			get{return _cclitype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ccliName
		{
			set{ _ccliname=value;}
			get{return _ccliname;}
		}
		#endregion Model

	}
}

