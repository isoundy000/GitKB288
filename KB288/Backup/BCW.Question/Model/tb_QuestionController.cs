using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类tb_QuestionController 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class tb_QuestionController
	{
		public tb_QuestionController()
		{}
		#region Model
		private int _id;
		private int? _muid;
		private string _uid;
		private int? _type;
		private int? _count;
		private string _list;
		private string _uided;
		private int? _acount;
		private int? _wcount;
		private string _ycount;
		private string _ubbtext;
		private int? _passtime;
		private int? _awardtype;
		private int? _awardptype;
		private int? _award;
		private DateTime? _addtime;
		private DateTime? _overtime;
		private string _remark;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 发布者ID
		/// </summary>
		public int? muid
		{
			set{ _muid=value;}
			get{return _muid;}
		}
		/// <summary>
		/// 记录可以回答的这次发布的问题的会员ID #分隔
		/// </summary>
		public string uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// 0所有会员可答 1在线会员答 3隐身会员答 4指定会员答
		/// </summary>
		public int? type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 本次问题个数
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 问题ID记录
		/// </summary>
		public string List
		{
			set{ _list=value;}
			get{return _list;}
		}
		/// <summary>
		/// 记录已作答会员ID
		/// </summary>
		public string uided
		{
			set{ _uided=value;}
			get{return _uided;}
		}
		/// <summary>
		/// 记录总的数量
		/// </summary>
		public int? acount
		{
			set{ _acount=value;}
			get{return _acount;}
		}
		/// <summary>
		/// 记录未答的数量
		/// </summary>
		public int? wcount
		{
			set{ _wcount=value;}
			get{return _wcount;}
		}
		/// <summary>
		/// 记录已答的数量
		/// </summary>
		public string ycount
		{
			set{ _ycount=value;}
			get{return _ycount;}
		}
		/// <summary>
		/// 发送的内线语句
		/// </summary>
		public string ubbtext
		{
			set{ _ubbtext=value;}
			get{return _ubbtext;}
		}
		/// <summary>
		/// 本轮问题过期时间
		/// </summary>
		public int? passtime
		{
			set{ _passtime=value;}
			get{return _passtime;}
		}
		/// <summary>
		/// 奖励形式  "0全答有奖(无论对不对,只要全答了)", "1全答对才有奖(全对)", "2答了就有奖(无论对不对)
		/// </summary>
		public int? awardtype
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// 发放奖励形式 0固定奖金 1随机红包
		/// </summary>
		public int? awardptype
		{
			set{ _awardptype=value;}
			get{return _awardptype;}
		}
		/// <summary>
		/// 范围值 0-x  若为固定奖金则直接派该值
		/// </summary>
		public int? award
		{
			set{ _award=value;}
			get{return _award;}
		}
		/// <summary>
		/// 加入时间
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

