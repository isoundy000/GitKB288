using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_QuestionsList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_QuestionsList
	{
		public tb_QuestionsList()
		{}
		#region Model
		private int _id;
		private string _question;
		private string _choosea;
		private string _chooseb;
		private string _choosec;
		private string _choosed;
		private string _answer;
		private string _styleid;
		private int? _style;
		private string _type;
		private int? _deficult;
		private int? _awardid;
		private string _awardtype;
		private int? _awardgold;
		private string _title;
		private string _img;
		private int? _hit;
		private int? _statue;
		private int? _trueanswer;
		private int? _falseanswer;
		private string _remarks;
		private string _indent;
		private DateTime? _addtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 问题
		/// </summary>
		public string question
		{
			set{ _question=value;}
			get{return _question;}
		}
		/// <summary>
		/// A选项
		/// </summary>
		public string chooseA
		{
			set{ _choosea=value;}
			get{return _choosea;}
		}
		/// <summary>
		/// B选项
		/// </summary>
		public string chooseB
		{
			set{ _chooseb=value;}
			get{return _chooseb;}
		}
		/// <summary>
		/// C选项
		/// </summary>
		public string chooseC
		{
			set{ _choosec=value;}
			get{return _choosec;}
		}
		/// <summary>
		/// D选项
		/// </summary>
		public string chooseD
		{
			set{ _choosed=value;}
			get{return _choosed;}
		}
		/// <summary>
		/// 问题正确答案
		/// </summary>
		public string answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 所属ID
		/// </summary>
		public string styleID
		{
			set{ _styleid=value;}
			get{return _styleid;}
		}
		/// <summary>
		/// 题目类型:1选择题(4选1)2(2选1)3(3选1)
		/// </summary>
		public int? style
		{
			set{ _style=value;}
			get{return _style;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 难度
		/// </summary>
		public int? deficult
		{
			set{ _deficult=value;}
			get{return _deficult;}
		}
		/// <summary>
		/// 答中奖品ID(0则不存在)
		/// </summary>
		public int? awardId
		{
			set{ _awardid=value;}
			get{return _awardid;}
		}
		/// <summary>
		/// 答中奖品类型
		/// </summary>
		public string awardType
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? awardGold
		{
			set{ _awardgold=value;}
			get{return _awardgold;}
		}
		/// <summary>
		/// 对应标题
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 是否有图片(0则无)
		/// </summary>
		public string img
		{
			set{ _img=value;}
			get{return _img;}
		}
		/// <summary>
		/// 点击次数
		/// </summary>
		public int? hit
		{
			set{ _hit=value;}
			get{return _hit;}
		}
		/// <summary>
		/// 问题状态(1可选0不可选)
		/// </summary>
		public int? statue
		{
			set{ _statue=value;}
			get{return _statue;}
		}
		/// <summary>
		/// 正确
		/// </summary>
		public int? trueAnswer
		{
			set{ _trueanswer=value;}
			get{return _trueanswer;}
		}
		/// <summary>
		/// 错误
		/// </summary>
		public int? falseAnswer
		{
			set{ _falseanswer=value;}
			get{return _falseanswer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string indent
		{
			set{ _indent=value;}
			get{return _indent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

