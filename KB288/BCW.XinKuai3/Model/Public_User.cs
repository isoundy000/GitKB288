using System;
namespace BCW.XinKuai3.Model
{
	/// <summary>
	/// ʵ����Public_User ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Public_User
	{
		public Public_User()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private string _settings;
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
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public string Settings
		{
			set{ _settings=value;}
			get{return _settings;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

