using System;
namespace BCW.Mobile.Model
{
	/// <summary>
	/// UserPlatform:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UserPlatform
	{
		public UserPlatform()
		{}
		#region Model
		private string _platformid;
		private int _platformtype;
		private int _userid;
		/// <summary>
		/// 
		/// </summary>
		public string platformId
		{
			set{ _platformid=value;}
			get{return _platformid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int platformType
		{
			set{ _platformtype=value;}
			get{return _platformtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int userId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		#endregion Model

	}
}

