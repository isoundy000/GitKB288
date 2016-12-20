using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Network 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Network
	{
		public Network()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _usid;
		private string _usname;
		private DateTime _overtime;
		private DateTime _addtime;
		private string _onids;
		private int _isubb;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型0不允许其它用户延时/1允许/2圈聊标识
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 广播标题
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 发布用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 发布用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 接收圈子提醒的ID
		/// </summary>
		public string OnIDs
		{
			set{ _onids=value;}
			get{return _onids;}
		}
		/// <summary>
		/// 是否支持UBB
		/// </summary>
		public int IsUbb
		{
			set{ _isubb=value;}
			get{return _isubb;}
		}
		#endregion Model

	}
}

