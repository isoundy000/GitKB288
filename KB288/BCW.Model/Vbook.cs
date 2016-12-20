using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Vbook 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Vbook
	{
		public Vbook()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _content;
		private string _sytext;
		private int _face;
		private int _usid;
		private string _usname;
		private string _addusip;
		private DateTime _addtime;
		private string _retext;
		private string _rename;
		private DateTime? _retime;
		private string _notes;
		private string _vpwd;
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
		/// 留言标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 留言内容
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 悄悄话
		/// </summary>
		public string SyText
		{
			set{ _sytext=value;}
			get{return _sytext;}
		}
		/// <summary>
		/// 表情
		/// </summary>
		public int Face
		{
			set{ _face=value;}
			get{return _face;}
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
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 操作IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回复昵称
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// 回复内容
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		/// <summary>
		/// 回复时间
		/// </summary>
		public DateTime? ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 留言密码
		/// </summary>
		public string VPwd
		{
			set{ _vpwd=value;}
			get{return _vpwd;}
		}
		#endregion Model

	}
}

