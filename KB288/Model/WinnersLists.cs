using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_WinnersLists 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_WinnersLists
	{
		public tb_WinnersLists()
		{}
		#region Model
		private long _id;
		private int? _awardid;
		private int? _userid;
		private int? _wingold;
		private int? _getid;
		private int? _fromid;
		private int? _tabelid;
		private string _gamename;
		private DateTime? _addtime;
		private int? _isget;
		private int? _overtime;
		private string _remarks;
		private int? _ident;
		/// <summary>
		/// 
		/// </summary>
		public long Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? awardId
		{
			set{ _awardid=value;}
			get{return _awardid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? winGold
		{
			set{ _wingold=value;}
			get{return _wingold;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GetId
		{
			set{ _getid=value;}
			get{return _getid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FromId
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TabelId
		{
			set{ _tabelid=value;}
			get{return _tabelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GameName
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
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
		public int? overTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
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
		public int? Ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		#endregion Model

	}
}

