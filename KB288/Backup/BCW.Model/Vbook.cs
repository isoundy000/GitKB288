using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Vbook ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Vbook
	{
		public Vbook()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _content;
		private string _sytext;
		private int _face;
		private int _usid;
		private string _usname;
		private string _addusip;
		private DateTime _addtime;
		private string _retext;
		private string _rename;
		private DateTime? _retime;
		private string _notes;
		private string _vpwd;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ���Ա���
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// ���Ļ�
		/// </summary>
		public string SyText
		{
			set{ _sytext=value;}
			get{return _sytext;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Face
		{
			set{ _face=value;}
			get{return _face;}
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
		/// ����IP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �ظ��ǳ�
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// �ظ�����
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		/// <summary>
		/// �ظ�ʱ��
		/// </summary>
		public DateTime? ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// ��ע
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string VPwd
		{
			set{ _vpwd=value;}
			get{return _vpwd;}
		}
		#endregion Model

	}
}

