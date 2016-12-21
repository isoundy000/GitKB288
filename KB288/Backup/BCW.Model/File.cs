using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����File ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class File
	{
		public File()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _nodeid;
		private string _files;
		private string _prevfiles;
		private string _content;
		private long _filesize;
		private string _fileext;
		private int _downnum;
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
		/// �������ͣ�1ͼƬ/2�ļ���
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
		/// �ļ���ַ
		/// </summary>
		public string Files
		{
			set{ _files=value;}
			get{return _files;}
		}
		/// <summary>
		/// ����ͼ�ļ���ַ(ֻ���ͼƬ����)
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
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

