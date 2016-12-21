using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Role ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Role
	{
		public Role()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private string _rolece;
		private string _rolename;
		private int _forumid;
		private string _forumname;
		private DateTime _starttime;
		private DateTime _overtime;
		private int _include;
		private int _status;
		private string _addname;
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
		/// �û�Ȩ��
		/// </summary>
		public string Rolece
		{
			set{ _rolece=value;}
			get{return _rolece;}
		}
		/// <summary>
		/// Ȩ��ְ��
		/// </summary>
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// �����ĳ��̳ID
		/// </summary>
		public int ForumID
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// �����ĳ��̳����
		/// </summary>
		public string ForumName
		{
			set{ _forumname=value;}
			get{return _forumname;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// �Ƿ�����¼����
		/// </summary>
		public int Include
		{
			set{ _include=value;}
			get{return _include;}
		}
		/// <summary>
		/// ����״̬��0����/1��ͣ��/2�����棩
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string AddName
		{
			set{ _addname=value;}
			get{return _addname;}
		}
		#endregion Model

	}
}

