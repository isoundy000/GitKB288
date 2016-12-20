using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Upfile ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Upfile
	{
		public Upfile()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private int _usid;
		private int _forumid;
		private int _bid;
		private int _reid;
		private string _files;
		private string _prevfiles;
		private string _content;
		private long _filesize;
		private string _fileext;
		private int _downnum;
		private int _cent;
		private int _isverify;
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
		/// ����:1���-2����-3��Ƶ-4��Դ
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ����ļ���ID
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
		/// ����ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// �ļ���ַ
		/// </summary>
		public string Files
		{
			set{ _files=value;}
			get{return _files;}
		}
		/// <summary>
		/// ����ͼ�ļ���ַ
		/// </summary>
		public string PrevFiles
		{
			set{ _prevfiles=value;}
			get{return _prevfiles;}
		}
		/// <summary>
		/// �ļ�����
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// �ļ���С
		/// </summary>
		public long FileSize
		{
			set{ _filesize=value;}
			get{return _filesize;}
		}
		/// <summary>
		/// �ļ���ʽ
		/// </summary>
		public string FileExt
		{
			set{ _fileext=value;}
			get{return _fileext;}
		}
		/// <summary>
		/// ���ش���
		/// </summary>
		public int DownNum
		{
			set{ _downnum=value;}
			get{return _downnum;}
		}
		/// <summary>
		/// �շѱ���
		/// </summary>
		public int Cent
		{
			set{ _cent=value;}
			get{return _cent;}
		}
		/// <summary>
		/// �Ƿ������(0�����/1δ���)
		/// </summary>
		public int IsVerify
		{
			set{ _isverify=value;}
			get{return _isverify;}
		}
		/// <summary>
		/// �ϴ�ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

