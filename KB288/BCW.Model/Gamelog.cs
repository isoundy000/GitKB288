using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Gamelog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Gamelog
	{
		public Gamelog()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _content;
		private string _notes;
		private int _enid;
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
		/// 游戏类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 日志内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 管理员输入备注
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 赛事ID
		/// </summary>
		public int EnId
		{
			set{ _enid=value;}
			get{return _enid;}
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

