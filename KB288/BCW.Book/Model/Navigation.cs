using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����Navigation ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Navigation
	{
		public Navigation()
		{}
		#region Model
		private int _id;
		private int _pid;
		private string _name;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int pid
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		#endregion Model

	}
}

