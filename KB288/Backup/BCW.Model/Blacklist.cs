using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Blacklist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Blacklist
	{
		public Blacklist()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _forumid;
		private string _forumname;
		private string _blackrole;
		private string _blackwhy;
		private int _blackday;
		private int _include;
		private int _adminusid;
		private DateTime _exittime;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
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
		public string ForumName
		{
			set{ _forumname=value;}
			get{return _forumname;}
		}
		/// <summary>
		/// �Ӻڵ�Ȩ��
		/// </summary>
		public string BlackRole
		{
			set{ _blackrole=value;}
			get{return _blackrole;}
		}
		/// <summary>
		/// �Ӻ�����
		/// </summary>
		public string BlackWhy
		{
			set{ _blackwhy=value;}
			get{return _blackwhy;}
		}
		/// <summary>
		/// �Ӻڵ�����
		/// </summary>
		public int BlackDay
		{
			set{ _blackday=value;}
			get{return _blackday;}
		}
		/// <summary>
		/// �Ӻ��Ƿ�����¼����
		/// </summary>
		public int Include
		{
			set{ _include=value;}
			get{return _include;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int AdminUsID
		{
			set{ _adminusid=value;}
			get{return _adminusid;}
		}
		/// <summary>
		/// �Զ����ʱ��
		/// </summary>
		public DateTime ExitTime
		{
			set{ _exittime=value;}
			get{return _exittime;}
		}
		/// <summary>
		/// �Ӻ�ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

