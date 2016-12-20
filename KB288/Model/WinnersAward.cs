using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_WinnersAward 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_WinnersAward
	{
		public tb_WinnersAward()
		{}
		#region Model
		private int _id;
		private int? _periods;
		private int? _awardnumber;
		private int? _awardnowcount;
		private int? _winnumber;
		private string _winnowcount;
		private string _getusid;
		private string _getwinnumber;
		private int? _identification;
		private string _remarks;
		private int? _isget;
		private DateTime? _addtime;
		private DateTime? _overtime;
		private string _award;
		private int? _isdone;
		private string _getredy;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? periods
		{
			set{ _periods=value;}
			get{return _periods;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? awardNumber
		{
			set{ _awardnumber=value;}
			get{return _awardnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? awardNowCount
		{
			set{ _awardnowcount=value;}
			get{return _awardnowcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? winNumber
		{
			set{ _winnumber=value;}
			get{return _winnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string winNowCount
		{
			set{ _winnowcount=value;}
			get{return _winnowcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string getUsId
		{
			set{ _getusid=value;}
			get{return _getusid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string getWinNumber
		{
			set{ _getwinnumber=value;}
			get{return _getwinnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? identification
		{
			set{ _identification=value;}
			get{return _identification;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isGet
		{
			set{ _isget=value;}
			get{return _isget;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? overTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string award
		{
			set{ _award=value;}
			get{return _award;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string getRedy
		{
			set{ _getredy=value;}
			get{return _getredy;}
		}
		#endregion Model

	}
}

