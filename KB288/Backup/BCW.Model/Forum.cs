using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Forum ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Forum
	{
		public Forum()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private string _donode;
		private string _title;
		private string _notes;
		private string _logo;
		private string _content;
		private string _label;
		private int _postlt;
		private int _replylt;
		private int _gradelt;
		private int _visitlt;
		private int _showtype;
		private int _isnode;
		private int _isactive;
		private int _groupid;
		private string _visitid;
		private int _ispc;
		private int _line;
		private int _topline;
		private DateTime _toptime;
		private string _topubb;
		private string _footubb;
		private int _paixu;
		private long _icent;
		/// <summary>
		/// ��̳ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �ڵ�ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// �¼����ID����
		/// </summary>
		public string DoNode
		{
			set{ _donode=value;}
			get{return _donode;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ���ں�
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// LOGOͼƬ��ַ
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// ��湫��
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// ���ӷ����ǩ
		/// </summary>
		public string Label
		{
			set{ _label=value;}
			get{return _label;}
		}
		/// <summary>
		/// �������ƣ�0�����ƣ�1�ް�����2����Ա��3��ֹ������
		/// </summary>
		public int Postlt
		{
			set{ _postlt=value;}
			get{return _postlt;}
		}
		/// <summary>
		/// �ظ����ƣ�ͬ�ϣ�
		/// </summary>
		public int Replylt
		{
			set{ _replylt=value;}
			get{return _replylt;}
		}
		/// <summary>
		/// �޶��ٵȼ��ɽ�
		/// </summary>
		public int Gradelt
		{
			set{ _gradelt=value;}
			get{return _gradelt;}
		}
		/// <summary>
		/// �������ƣ�0�����ƣ�1���ƻ�Ա��2VIP��Ա��3���ư�����4���ƹ���Ա��
		/// </summary>
		public int Visitlt
		{
			set{ _visitlt=value;}
			get{return _visitlt;}
		}
		/// <summary>
		/// ��ʾ���ӷ�ʽ  0|ֻ�Ա�����|1|��ʾ�¼���
		/// </summary>
		public int ShowType
		{
			set{ _showtype=value;}
			get{return _showtype;}
		}
		/// <summary>
		/// �Ƿ���ʾ�¼���� 0|����ʾ|1|��ʾ
		/// </summary>
		public int IsNode
		{
			set{ _isnode=value;}
			get{return _isnode;}
		}
		/// <summary>
		/// 0������1��ͣ
		/// </summary>
		public int IsActive
		{
			set{ _isactive=value;}
			get{return _isactive;}
		}
		/// <summary>
		/// ����Ȧ��ID
		/// </summary>
		public int GroupId
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// ��������ID��|�ֿ���
		/// </summary>
		public string VisitId
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// ���Է���
		/// </summary>
		public int IsPc
		{
			set {_ispc=value;}
			get {return _ispc;}
		}
		/// <summary>
		/// ��ǰ����
		/// </summary>
		public int Line
		{
			set{ _line=value;}
			get{return _line;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public int TopLine
		{
			set{ _topline=value;}
			get{return _topline;}
		}
		/// <summary>
		/// �������ʱ��
		/// </summary>
		public DateTime TopTime
		{
			set{ _toptime=value;}
			get{return _toptime;}
		}
		/// <summary>
		/// ��̳����UBB
		/// </summary>
		public string TopUbb
		{
			set{ _topubb=value;}
			get{return _topubb;}
		}
		/// <summary>
		/// ��̳�ײ�UBB
		/// </summary>
		public string FootUbb
		{
			set{ _footubb=value;}
			get{return _footubb;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// ������Ŀ
		/// </summary>
		public long iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		#endregion Model

	}
}

