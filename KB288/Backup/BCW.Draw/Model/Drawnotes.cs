using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类Drawnotes 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Drawnotes
	{
		public Drawnotes()
		{}
		#region Model
		private int _id;
		private int _usid;
		private long _jifen;
		private string _game;
		private string _gname;
		private long _gvalue;
		private long _jvalue;
		private long _change;
		private DateTime _date;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// 积分
		/// </summary>
		public long jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
		/// <summary>
		/// 来源的游戏
		/// </summary>
		public string game
		{
			set{ _game=value;}
			get{return _game;}
		}
		/// <summary>
		/// 来源的变量
		/// </summary>
		public string gname
		{
			set{ _gname=value;}
			get{return _gname;}
		}
		/// <summary>
		/// 兑换币值
		/// </summary>
		public long gvalue
		{
			set{ _gvalue=value;}
			get{return _gvalue;}
		}
		/// <summary>
		/// 积分兑换值
		/// </summary>
		public long jvalue
		{
			set{ _jvalue=value;}
			get{return _jvalue;}
		}
		/// <summary>
		/// 兑换的汇率
		/// </summary>
		public long change
		{
			set{ _change=value;}
			get{return _change;}
		}
		/// <summary>
		/// 兑换时间
		/// </summary>
		public DateTime date
		{
			set{ _date=value;}
			get{return _date;}
		}
		#endregion Model

	}
}

