using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类MarryAction 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class MarryAction
	{
		public MarryAction()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private string _content;
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
		/// 结婚ID
		/// </summary>
		public int MarryId
		{
			set{ _marryid=value;}
			get{return _marryid;}
		}
		/// <summary>
		/// 生活点滴内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

