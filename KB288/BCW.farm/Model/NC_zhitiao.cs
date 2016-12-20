using System;
namespace BCW.farm.Model
{
	/// <summary>
	/// 实体类NC_zhitiao 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class NC_zhitiao
	{
		public NC_zhitiao()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _contact;
		private DateTime _addtime;
		private int _type;
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
		/// 内容
		/// </summary>
		public string contact
		{
			set{ _contact=value;}
			get{return _contact;}
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
		/// 0投诉1建议2通过
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

