using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_QuestionAnswer 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_QuestionAnswer
	{
		public tb_QuestionAnswer()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private int? _questid;
		private string _questtion;
		private string _answer;
		private int? _istrue;
		private int? _ishit;
		private int? _isget;
		private string _gettype;
		private long _getgold;
		private int? _isdone;
		private DateTime? _addtime;
		private int? _needtime;
		private int? _isover;
		private int? _ident;
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
		public int? usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string usname
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 问题ID
		/// </summary>
		public int? questID
		{
			set{ _questid=value;}
			get{return _questid;}
		}
		/// <summary>
		/// 问题
		/// </summary>
		public string questtion
		{
			set{ _questtion=value;}
			get{return _questtion;}
		}
		/// <summary>
		/// 回答
		/// </summary>
		public string answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 是否正确
		/// </summary>
		public int? isTrue
		{
			set{ _istrue=value;}
			get{return _istrue;}
		}
		/// <summary>
		/// 是否隐藏
		/// </summary>
		public int? isHit
		{
			set{ _ishit=value;}
			get{return _ishit;}
		}
		/// <summary>
		/// 是否获得的奖励ID 0则无
		/// </summary>
		public int? isGet
		{
			set{ _isget=value;}
			get{return _isget;}
		}
		/// <summary>
		/// 获得的类型
		/// </summary>
		public string getType
		{
			set{ _gettype=value;}
			get{return _gettype;}
		}
		/// <summary>
		/// 获得的币值
		/// </summary>
		public long getGold
		{
			set{ _getgold=value;}
			get{return _getgold;}
		}
		/// <summary>
		/// 是否回答了 0否1已回答
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回答秒数
		/// </summary>
		public int? needTime
		{
			set{ _needtime=value;}
			get{return _needtime;}
		}
		/// <summary>
		/// 是否超时
		/// </summary>
		public int? isOver
		{
			set{ _isover=value;}
			get{return _isover;}
		}
		/// <summary>
		/// 记录标识
		/// </summary>
		public int? ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		#endregion Model

	}
}

