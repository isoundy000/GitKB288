using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_QuestionsAnswerCtr 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_QuestionsAnswerCtr
	{
		public tb_QuestionsAnswerCtr()
		{}
		#region Model
		private int _id;
		private int? _uid;
		private int? _contrid;
		private string _list;
		private string _answerlist;
		private int? _count;
		private int? _now;
		private int? _awardtype;
		private int? _awardid;
		private string _explain;
		private int? _awardgold;
		private int? _truecount;
		private int? _flasecount;
		private int? _ishit;
		private int? _isdone;
		private DateTime? _addtime;
		private DateTime? _overtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public int? uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// 问题控制器id
		/// </summary>
		public int? contrID
		{
			set{ _contrid=value;}
			get{return _contrid;}
		}
		/// <summary>
		/// 问题id列
		/// </summary>
		public string List
		{
			set{ _list=value;}
			get{return _list;}
		}
		/// <summary>
		/// 已回答id列
		/// </summary>
		public string answerList
		{
			set{ _answerlist=value;}
			get{return _answerlist;}
		}
		/// <summary>
		/// 总数
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 当前回答数
		/// </summary>
		public int? now
		{
			set{ _now=value;}
			get{return _now;}
		}
		/// <summary>
		/// 奖品类型
		/// </summary>
		public int? awardtype
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// 奖品id
		/// </summary>
		public int? awardId
		{
			set{ _awardid=value;}
			get{return _awardid;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string explain
		{
			set{ _explain=value;}
			get{return _explain;}
		}
		/// <summary>
		/// 获得币值
		/// </summary>
		public int? awardgold
		{
			set{ _awardgold=value;}
			get{return _awardgold;}
		}
		/// <summary>
		/// 答对次数
		/// </summary>
		public int? trueCount
		{
			set{ _truecount=value;}
			get{return _truecount;}
		}
		/// <summary>
		/// 打错次数
		/// </summary>
		public int? flaseCount
		{
			set{ _flasecount=value;}
			get{return _flasecount;}
		}
		/// <summary>
		/// 是否隐藏
		/// </summary>
		public int? ishit
		{
			set{ _ishit=value;}
			get{return _ishit;}
		}
		/// <summary>
		/// 是否过期
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 回答时间
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		#endregion Model

	}
}

