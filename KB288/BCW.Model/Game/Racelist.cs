using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Racelist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Racelist
	{
		public Racelist()
		{}
		#region Model
		private int _id;
		private string _payname;
		private int _payusid;
		private long _paycent;
		private DateTime _paytime;
		private int _raceid;
		private int _paytype;
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
		public string payname
		{
			set{ _payname=value;}
			get{return _payname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int payusid
		{
			set{ _payusid=value;}
			get{return _payusid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long payCent
		{
			set{ _paycent=value;}
			get{return _paycent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime paytime
		{
			set{ _paytime=value;}
			get{return _paytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int raceid
		{
			set{ _raceid=value;}
			get{return _raceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int paytype
		{
			set{ _paytype=value;}
			get{return _paytype;}
		}
		#endregion Model

	}
}

