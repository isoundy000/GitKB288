using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Friend ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Friend
	{
		public Friend()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private string _usname;
		private int _friendid;
		private string _friendname;
		private DateTime? _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���0/����/1/������/2/��ע��
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
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
		/// ����ID
		/// </summary>
		public int FriendID
		{
			set{ _friendid=value;}
			get{return _friendid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string FriendName
		{
			set{ _friendname=value;}
			get{return _friendname;}
		}
		/// <summary>
		/// �����ϵʱ��
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

