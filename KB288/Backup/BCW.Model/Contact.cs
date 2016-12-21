using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Contact 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Contact
	{
		public Contact()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private int _usid;
		private string _name;
		private string _mobile;
		private string _homephone;
		private string _officephone;
		private string _fax;
		private string _email;
		private string _company;
		private string _posit;
		private string _content;
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
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
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
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HomePhone
		{
			set{ _homephone=value;}
			get{return _homephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OfficePhone
		{
			set{ _officephone=value;}
			get{return _officephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Fax
		{
			set{ _fax=value;}
			get{return _fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Company
		{
			set{ _company=value;}
			get{return _company;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Posit
		{
			set{ _posit=value;}
			get{return _posit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		#endregion Model

	}
}

