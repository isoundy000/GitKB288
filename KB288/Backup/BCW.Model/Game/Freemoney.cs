using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类Freemoney 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Freemoney
	{
		public Freemoney()
		{}
		#region Model
		private int _id;
		private int? _zid;
		private int? _kid;
		private string _kname;
		private decimal? _winmoney;
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
		public int? zid
		{
			set{ _zid=value;}
			get{return _zid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? kid
		{
			set{ _kid=value;}
			get{return _kid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string kname
		{
			set{ _kname=value;}
			get{return _kname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? winmoney
		{
			set{ _winmoney=value;}
			get{return _winmoney;}
		}
		#endregion Model

	}
}

