using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Applepay 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Applepay
	{
		public Applepay()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _appleid;
		private int _paycount;
		private int _usid;
		private string _usname;
		private long _wincent;
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
		/// 类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 苹果ID
		/// </summary>
		public int AppleId
		{
			set{ _appleid=value;}
			get{return _appleid;}
		}
		/// <summary>
		/// 下注数额
		/// </summary>
		public int PayCount
		{
			set{ _paycount=value;}
			get{return _paycount;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 赢币数量
		/// </summary>
		public long WinCent
		{
			set{ _wincent=value;}
			get{return _wincent;}
		}
		/// <summary>
		/// 状态
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

