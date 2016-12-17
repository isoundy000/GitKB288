using System;
namespace BCW.MobileSlider.Model
{
	/// <summary>
	/// MobileSlider:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class MobileSlider
	{
		public MobileSlider()
		{}
		#region Model
		private int _id;
		private string _url;
		private string _contenttype;
		private string _param;
		private int? _sortid=0;
		private int _ptype;
		/// <summary>
		/// 自增标识
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// URL地址
		/// </summary>
		public string url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 内容类型
		/// </summary>
		public string contentType
		{
			set{ _contenttype=value;}
			get{return _contenttype;}
		}
		/// <summary>
		/// 附带参数
		/// </summary>
		public string param
		{
			set{ _param=value;}
			get{return _param;}
		}
		/// <summary>
		/// 排序ID
		/// </summary>
		public int? sortid
		{
			set{ _sortid=value;}
			get{return _sortid;}
		}
		/// <summary>
		/// 所属版块
		/// </summary>
		public int ptype
		{
			set{ _ptype=value;}
			get{return _ptype;}
		}
		#endregion Model

	}
}

