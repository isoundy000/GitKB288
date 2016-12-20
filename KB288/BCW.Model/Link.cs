using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Link 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Link
	{
		public Link()
		{}
		#region Model
		private int _id;
		private string _linkname;
		private string _linknamt;
		private string _linkurl;
		private string _linknotes;
		private string _keyword;
		private int _linkin;
		private int _linkout;
		private string _restats;
		private string _relastip;
		private DateTime _linktime;
		private DateTime? _linktime2;
		private DateTime _addtime;
		private int _leibie;
		private int _linkrd;
		private int _hidden;
		private int _inonly;
		private int _todayin;
		private int _yesterdayin;
		private int _beforein;
		private int _todayout;
		private int _yesterdayout;
		private int _beforeout;
		private int _linkipin;
		private int _linkipout;
		private int _iptodayin;
		private int _ipyesterdayin;
		private int _ipbeforein;
		private int _iptodayout;
		private int _ipyesterdayout;
		private int _ipbeforeout;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 友链全称
		/// </summary>
		public string LinkName
		{
			set{ _linkname=value;}
			get{return _linkname;}
		}
		/// <summary>
		/// 友链简称
		/// </summary>
		public string LinkNamt
		{
			set{ _linknamt=value;}
			get{return _linknamt;}
		}
		/// <summary>
		/// 友链地址
		/// </summary>
		public string LinkUrl
		{
			set{ _linkurl=value;}
			get{return _linkurl;}
		}
		/// <summary>
		/// 简介
		/// </summary>
		public string LinkNotes
		{
			set{ _linknotes=value;}
			get{return _linknotes;}
		}
		/// <summary>
		/// 搜索关键词
		/// </summary>
		public string KeyWord
		{
			set{ _keyword=value;}
			get{return _keyword;}
		}
		/// <summary>
		/// 友链总链入PV
		/// </summary>
		public int LinkIn
		{
			set{ _linkin=value;}
			get{return _linkin;}
		}
		/// <summary>
		/// 友链总链出PV
		/// </summary>
		public int LinkOut
		{
			set{ _linkout=value;}
			get{return _linkout;}
		}
		/// <summary>
		/// 评价
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// 评价最后IP
		/// </summary>
		public string ReLastIP
		{
			set{ _relastip=value;}
			get{return _relastip;}
		}
		/// <summary>
		/// 链入时间
		/// </summary>
		public DateTime LinkTime
		{
			set{ _linktime=value;}
			get{return _linktime;}
		}
		/// <summary>
		/// 链出时间
		/// </summary>
		public DateTime? LinkTime2
		{
			set{ _linktime2=value;}
			get{return _linktime2;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 友链分类ID
		/// </summary>
		public int Leibie
		{
			set{ _leibie=value;}
			get{return _leibie;}
		}
		/// <summary>
		/// 是否推荐0|未推荐|1|已推荐
		/// </summary>
		public int LinkRd
		{
			set{ _linkrd=value;}
			get{return _linkrd;}
		}
		/// <summary>
		/// 是否审核0|未审核|1|已审核
		/// </summary>
		public int Hidden
		{
			set{ _hidden=value;}
			get{return _hidden;}
		}
		/// <summary>
		/// 有效PV
		/// </summary>
		public int InOnly
		{
			set{ _inonly=value;}
			get{return _inonly;}
		}
		/// <summary>
		/// 今天链入PV
		/// </summary>
		public int todayIn
		{
			set{ _todayin=value;}
			get{return _todayin;}
		}
		/// <summary>
		/// 昨天链入PV
		/// </summary>
		public int yesterdayIn
		{
			set{ _yesterdayin=value;}
			get{return _yesterdayin;}
		}
		/// <summary>
		/// 前天链入PV
		/// </summary>
		public int beforeIn
		{
			set{ _beforein=value;}
			get{return _beforein;}
		}
		/// <summary>
		/// 今天链出PV
		/// </summary>
		public int todayOut
		{
			set{ _todayout=value;}
			get{return _todayout;}
		}
		/// <summary>
		/// 昨天链出PV
		/// </summary>
		public int yesterdayOut
		{
			set{ _yesterdayout=value;}
			get{return _yesterdayout;}
		}
		/// <summary>
		/// 前天链出PV
		/// </summary>
		public int beforeOut
		{
			set{ _beforeout=value;}
			get{return _beforeout;}
		}
		/// <summary>
		/// 链入总IP
		/// </summary>
		public int LinkIPIn
		{
			set{ _linkipin=value;}
			get{return _linkipin;}
		}
		/// <summary>
		/// 链出总IP
		/// </summary>
		public int LinkIPOut
		{
			set{ _linkipout=value;}
			get{return _linkipout;}
		}
		/// <summary>
		/// 今天链入IP
		/// </summary>
		public int IPtodayIn
		{
			set{ _iptodayin=value;}
			get{return _iptodayin;}
		}
		/// <summary>
		/// 昨天链入IP
		/// </summary>
		public int IPyesterdayIn
		{
			set{ _ipyesterdayin=value;}
			get{return _ipyesterdayin;}
		}
		/// <summary>
		/// 前天链入IP
		/// </summary>
		public int IPbeforeIn
		{
			set{ _ipbeforein=value;}
			get{return _ipbeforein;}
		}
		/// <summary>
		/// 今天链出IP
		/// </summary>
		public int IPtodayOut
		{
			set{ _iptodayout=value;}
			get{return _iptodayout;}
		}
		/// <summary>
		/// 昨天链出IP
		/// </summary>
		public int IPyesterdayOut
		{
			set{ _ipyesterdayout=value;}
			get{return _ipyesterdayout;}
		}
		/// <summary>
		/// 前天链出IP
		/// </summary>
		public int IPbeforeOut
		{
			set{ _ipbeforeout=value;}
			get{return _ipbeforeout;}
		}
		#endregion Model

	}
}

