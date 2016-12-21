using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Toplist 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Toplist
	{
		public Toplist()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _usname;
		private long _wingold;
		private long _putgold;
		private int _winnum;
		private int _putnum;
		/// <summary>
		/// 排行自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 排行类别
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsId
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
		/// 赢得币
		/// </summary>
		public long WinGold
		{
			set{ _wingold=value;}
			get{return _wingold;}
		}
		/// <summary>
		/// 投入币
		/// </summary>
		public long PutGold
		{
			set{ _putgold=value;}
			get{return _putgold;}
		}
		/// <summary>
		/// 赢的次数
		/// </summary>
		public int WinNum
		{
			set{ _winnum=value;}
			get{return _winnum;}
		}
		/// <summary>
		/// 参与数次
		/// </summary>
		public int PutNum
		{
			set{ _putnum=value;}
			get{return _putnum;}
		}
		#endregion Model

	}
}

