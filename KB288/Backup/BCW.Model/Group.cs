using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Group ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Group
	{
		public Group()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _city;
		private string _logo;
		private string _notes;
		private string _content;
		private int _usid;
		private long _icent;
		private int _itotal;
		private string _visitid;
		private DateTime _visittime;
		private int _iclick;
		private int _intype;
		private int _forumid;
		private int _chatid;
		private int _forumstatus;
		private int _chatstatus;
		private string _signid;
		private DateTime _signtime;
		private int _signcent;
		private string _icentpwd;
		private int _status;
		private DateTime _extime;
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
		/// Ȧ������
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// Ȧ������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// Ȧ�ӻ���
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// Ȧ������
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ����ԭ��
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public long iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// ��Ա����
		/// </summary>
		public int iTotal
		{
			set{ _itotal=value;}
			get{return _itotal;}
		}
		/// <summary>
		/// �������ID
		/// </summary>
		public string VisitId
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime VisitTime
		{
			set{ _visittime=value;}
			get{return _visittime;}
		}
		/// <summary>
		/// �ۻ�����
		/// </summary>
		public int iClick
		{
			set{ _iclick=value;}
			get{return _iclick;}
		}
		/// <summary>
		/// ����Ȧ�����ƣ�0������/1Ҫ��֤/2��������룩
		/// </summary>
		public int InType
		{
			set{ _intype=value;}
			get{return _intype;}
		}
		/// <summary>
		/// ������̳ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// ��������ID
		/// </summary>
		public int ChatId
		{
			set{ _chatid=value;}
			get{return _chatid;}
		}
		/// <summary>
		/// ��̳���أ�0����/1��ͣ��
		/// </summary>
		public int ForumStatus
		{
			set{ _forumstatus=value;}
			get{return _forumstatus;}
		}
		/// <summary>
		/// Ȧ�Ŀ��أ�0����/1��ͣ��
		/// </summary>
		public int ChatStatus
		{
			set{ _chatstatus=value;}
			get{return _chatstatus;}
		}
		/// <summary>
		/// ǩ��ID
		/// </summary>
		public string SignID
		{
			set{ _signid=value;}
			get{return _signid;}
		}
		/// <summary>
		/// ǩ��ʱ��
		/// </summary>
		public DateTime SignTime
		{
			set{ _signtime=value;}
			get{return _signtime;}
		}
		/// <summary>
		/// ǩ���ñ���Ŀ
		/// </summary>
		public int SignCent
		{
			set{ _signcent=value;}
			get{return _signcent;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string iCentPwd
		{
			set{ _icentpwd=value;}
			get{return _icentpwd;}
		}
		/// <summary>
		/// Ȧ��״̬(0����/1δ����)
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

