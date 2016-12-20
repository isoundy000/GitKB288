using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Gameowe 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Gameowe
	{
		public Gameowe()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private string _content;
		private long _owecent;
		private int _enid;
		private int _bztype;
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
		/// 差币原因
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 差币数额
		/// </summary>
		public long OweCent
		{
			set{ _owecent=value;}
			get{return _owecent;}
		}
		/// <summary>
		/// 游戏ID
		/// </summary>
		public int EnId
		{
			set{ _enid=value;}
			get{return _enid;}
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
		/// 添加时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

