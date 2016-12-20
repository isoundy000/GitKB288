using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类SSClist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class SSClist
	{
		public SSClist()
		{}
		#region Model
		private int _id;
		private int _sscid;
		private string _result;
		private string _notes;
		private int _state;
		private DateTime _endtime;
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
		public int SSCId
		{
			set{ _sscid=value;}
			get{return _sscid;}
		}
		/// <summary>
		/// 开奖结果:1 2 3 4 5
		/// </summary>
		public string Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 保留
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 状态（0正常，1已有结果）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 代购截止时间
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

