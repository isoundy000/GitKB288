using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Shoptg 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Shoptg
	{
		public Shoptg()
		{}
		#region Model
		private int _id;
		private int _zrid;
		private int _usid;
		private string _usname;
		private string _notes;
		private int _detailid;
		private DateTime _addtime;
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
		public int ZrID
		{
			set{ _zrid=value;}
			get{return _zrid;}
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
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int DetailId
		{
			set{ _detailid=value;}
			get{return _detailid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

