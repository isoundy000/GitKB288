using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Forumts ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Forumts
	{
		public Forumts()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _forumid;
		private int _paixu;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ר�����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��̳ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// ��̳����
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		#endregion Model

	}
}

