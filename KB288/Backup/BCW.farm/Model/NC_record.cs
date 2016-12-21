using System;
namespace BCW.farm.Model
{
	/// <summary>
	/// 实体类NC_record 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class NC_record
	{
		public NC_record()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _text;
		private string _ip;
		private DateTime _addtime;
		private string _browser;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string text
		{
			set{ _text=value;}
			get{return _text;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Browser
		{
			set{ _browser=value;}
			get{return _browser;}
		}
		#endregion Model

	}
}

