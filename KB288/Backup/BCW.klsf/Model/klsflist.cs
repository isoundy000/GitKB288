using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类klsflist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class klsflist
	{
		public klsflist()
		{}
		#region Model
		private int _id;
		private int _klsfid;
		private string _result;
		private string _notes;
		private int _state;
		private DateTime _endtime;
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
		public int klsfId
		{
			set{ _klsfid=value;}
			get{return _klsfid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

