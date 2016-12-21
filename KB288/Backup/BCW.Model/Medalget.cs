using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Medalget 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Medalget
	{
		public Medalget()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private int _medalid;
		private string _notes;
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
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 勋章ID
		/// </summary>
		public int MedalId
		{
			set{ _medalid=value;}
			get{return _medalid;}
		}
		/// <summary>
		/// 赠送内容
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
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

