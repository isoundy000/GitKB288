using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����ShuFav ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ShuFav
	{
		public ShuFav()
		{}
		#region Model
		private int _id;
		private string _name;
		private int _usid;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		#endregion Model

	}
}

