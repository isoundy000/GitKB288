using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Modata 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Modata
	{
		public Modata()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _phonebrand;
		private string _phonemodel;
		private string _phonesystem;
		private string _phonesize;
		private int _phoneclick;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 品牌类型ID
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 品牌名称
		/// </summary>
		public string PhoneBrand
		{
			set{ _phonebrand=value;}
			get{return _phonebrand;}
		}
		/// <summary>
		/// 手机型号
		/// </summary>
		public string PhoneModel
		{
			set{ _phonemodel=value;}
			get{return _phonemodel;}
		}
		/// <summary>
		/// 操作系统
		/// </summary>
		public string PhoneSystem
		{
			set{ _phonesystem=value;}
			get{return _phonesystem;}
		}
		/// <summary>
		/// 屏幕分辨率
		/// </summary>
		public string PhoneSize
		{
			set{ _phonesize=value;}
			get{return _phonesize;}
		}
		/// <summary>
		/// 选定次数
		/// </summary>
		public int PhoneClick
		{
			set{ _phoneclick=value;}
			get{return _phoneclick;}
		}
		#endregion Model

	}
}

