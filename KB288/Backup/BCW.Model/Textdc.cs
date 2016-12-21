using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Textdc 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Textdc
	{
		public Textdc()
		{}
		#region Model
		private int _id;
		private int _bid;
		private int _usid;
		private long _outcent;
		private long _accent;
		private int _isztid;
		private int _bztype;
		private int _state;
		private DateTime _addtime;
		private string _logtext;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 帖子ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// 下注额
		/// </summary>
		public long OutCent
		{
			set{ _outcent=value;}
			get{return _outcent;}
		}
		/// <summary>
		/// 确认币额（正数为庄赢，负数为闲赢）
		/// </summary>
		public long AcCent
		{
			set{ _accent=value;}
			get{return _accent;}
		}
		/// <summary>
		/// 0帖主ID/1闲家ID
		/// </summary>
		public int IsZtid
		{
			set{ _isztid=value;}
			get{return _isztid;}
		}
		/// <summary>
		/// 币种
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// 状态（0未结束/1闲赢确认中/2庄赢确认中/3已结束）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 日志记录（|分开）
		/// </summary>
		public string LogText
		{
			set{ _logtext=value;}
			get{return _logtext;}
		}
		#endregion Model

	}
}

