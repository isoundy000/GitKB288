using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Forumvote ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Forumvote
	{
		public Forumvote()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _qinum;
		private int _forumid;
		private int _bid;
		private int _usid;
		private string _usname;
		private string _notes;
		private DateTime _addtime;
		private int _iswin;
		private int _state;
		private DateTime _actime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���ͣ�1��Ф/2��ɫ/3ƽ��һФ/4����/5��β/6�岻�У�
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int qiNum
		{
			set{ _qinum=value;}
			get{return _qinum;}
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
		/// ����ID
		/// </summary>
		public int BID
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// ����
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// Ͷעʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �Ƿ��н�(0δ��/1�н���
		/// </summary>
		public int IsWin
		{
			set{ _iswin=value;}
			get{return _iswin;}
		}
		/// <summary>
		/// ״̬��0δ����/1�ѿ�����
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AcTime
		{
			set{ _actime=value;}
			get{return _actime;}
		}
		#endregion Model

	}
}

