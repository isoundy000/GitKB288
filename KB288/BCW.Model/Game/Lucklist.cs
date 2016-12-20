using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Lucklist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Lucklist
	{
		public Lucklist()
		{}
		#region Model
		private int _id;
		private int _sumnum;
		private string _postnum;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _luckcent;
		private long _pool;
		private long _beforepool;
		private int _state;
        private string _panduan;
        private int _bjkl8qihao;
        private string _bjkl8nums;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 北京快乐8期数
        /// </summary>
        public int Bjkl8Qihao
        {
            set { _bjkl8qihao = value; }
            get { return _bjkl8qihao; }
        }
        /// <summary>
        /// 北京快乐8开奖号码
        /// </summary>
        public string Bjkl8Nums
        {
            set { _bjkl8nums = value; }
            get { return _bjkl8nums; }
        }
        /// <summary>
        /// 判断是都增加期数标识
        /// </summary>
        public string panduan
        {
            set { _panduan = value; }
            get { return _panduan; }
        }
		/// <summary>
		/// 开奖结果数字
		/// </summary>
		public int SumNum
		{
			set{ _sumnum=value;}
			get{return _sumnum;}
		}
		/// <summary>
		/// 开奖数字组合
		/// </summary>
		public string PostNum
		{
			set{ _postnum=value;}
			get{return _postnum;}
		}
		/// <summary>
		/// 开奖时间
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 幸运数字下注总币
		/// </summary>
		public long LuckCent
		{
			set{ _luckcent=value;}
			get{return _luckcent;}
		}
		/// <summary>
		/// 奖池基金
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// 上期奖池落空基金
		/// </summary>
		public long BeforePool
		{
			set{ _beforepool=value;}
			get{return _beforepool;}
		}
		/// <summary>
		/// 状态 0未开/1已开奖
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

