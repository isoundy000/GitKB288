using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类LinkIp 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class LinkIp
	{
		public LinkIp()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _addusip;
		private string _addusua;
		private string _adduspage;
		private int _linkid;
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
		/// IP类型（0链入IP|1链出IP）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// IP地址
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// UA浏览器
		/// </summary>
		public string AddUsUA
		{
			set{ _addusua=value;}
			get{return _addusua;}
		}
		/// <summary>
		/// 来源地址
		/// </summary>
		public string AddUsPage
		{
			set{ _adduspage=value;}
			get{return _adduspage;}
		}
		/// <summary>
		/// 友链ID
		/// </summary>
		public int LinkId
		{
			set{ _linkid=value;}
			get{return _linkid;}
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

