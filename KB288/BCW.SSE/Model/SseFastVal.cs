using System;
namespace BCW.SSE.Model
{
	/// <summary>
	/// SseFastVal:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SseFastVal
	{
		public SseFastVal()
		{}
		#region Model
		private int _userid;
		private string _fastval;
		/// <summary>
		/// 
		/// </summary>
		public int userId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string fastVal
		{
			set{ _fastval=value;}
			get{return _fastval;}
		}
		#endregion Model

	}
}

