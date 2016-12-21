using System;
namespace LHC.Model
{
	/// <summary>
	/// 实体类VoteNo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class VoteNo
	{
		public VoteNo()
		{}
		#region Model
		private int _id;
		private int _qino;
		private DateTime _extime;
		private long _paycent;
		private int _paycount;
		private long _paycent2;
		private int _paycount2;
		private int _snum;
		private int _pnum1;
		private int _pnum2;
		private int _pnum3;
		private int _pnum4;
		private int _pnum5;
		private int _pnum6;
		private int _state;
		private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 期数
		/// </summary>
		public int qiNo
		{
			set{ _qino=value;}
			get{return _qino;}
		}
		/// <summary>
		/// 截止时间
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		/// <summary>
		/// 下注总额
		/// </summary>
		public long payCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 下注总数
		/// </summary>
		public int payCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 下注总额
		/// </summary>
		public long payCent2
		{
			set{ _paycent2=value;}
			get{return _paycent2;}
		}
		/// <summary>
		/// 下注总数
		/// </summary>
		public int payCount2
		{
			set{ _paycount2=value;}
			get{return _paycount2;}
		}
		/// <summary>
		/// 特别号码
		/// </summary>
		public int sNum
		{
			set{ _snum=value;}
			get{return _snum;}
		}
		/// <summary>
		/// 普通号码1
		/// </summary>
		public int pNum1
		{
			set{ _pnum1=value;}
			get{return _pnum1;}
		}
		/// <summary>
		/// 普通号码2
		/// </summary>
		public int pNum2
		{
			set{ _pnum2=value;}
			get{return _pnum2;}
		}
		/// <summary>
		/// 普通号码3
		/// </summary>
		public int pNum3
		{
			set{ _pnum3=value;}
			get{return _pnum3;}
		}
		/// <summary>
		/// 普通号码4
		/// </summary>
		public int pNum4
		{
			set{ _pnum4=value;}
			get{return _pnum4;}
		}
		/// <summary>
		/// 普通号码5
		/// </summary>
		public int pNum5
		{
			set{ _pnum5=value;}
			get{return _pnum5;}
		}
		/// <summary>
		/// 普通号码6
		/// </summary>
		public int pNum6
		{
			set{ _pnum6=value;}
			get{return _pnum6;}
		}
		/// <summary>
		/// 状态(0未开奖/1已开奖)
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

