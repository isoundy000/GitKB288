using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类SysTemp 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SysTemp
	{
		public SysTemp()
		{}
		#region Model
		private int _id;
		private DateTime _guessoddstime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 8波水位更新时间
		/// </summary>
		public DateTime GuessOddsTime
		{
			set{ _guessoddstime=value;}
			get{return _guessoddstime;}
		}
		#endregion Model

	}
}

