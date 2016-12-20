using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Medal ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Medal
	{
		public Medal()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private string _imageurl;
		private string _notes;
		private int _icent;
		private int _icount;
		private int _iday;
		private string _payid;
		private string _payextime;
		private int _paixu;
		private int _forumid;
		private string _payidtemp;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ѫ������
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ѫ������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ѫ��ͼƬ��ַ
		/// </summary>
		public string ImageUrl
		{
			set{ _imageurl=value;}
			get{return _imageurl;}
		}
		/// <summary>
		/// ѫ��˵��
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// �շѱ���
		/// </summary>
		public int iCent
		{
			set{ _icent=value;}
			get{return _icent;}
		}
		/// <summary>
		/// �����ʣ������
		/// </summary>
		public int iCount
		{
			set{ _icount=value;}
			get{return _icount;}
		}
		/// <summary>
		/// ��Ч����
		/// </summary>
		public int iDay
		{
			set{ _iday=value;}
			get{return _iday;}
		}
		/// <summary>
		/// ʹ�õ�ID����#�ֿ���
		/// </summary>
		public string PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// ����ʱ�䣨��#�ֿ���
		/// </summary>
		public string PayExTime
		{
			set{ _payextime=value;}
			get{return _payextime;}
		}
		/// <summary>
		/// ��̨����
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		/// <summary>
		/// ��̳ID
		/// </summary>
		public int ForumId
		{
			set{ _forumid=value;}
			get{return _forumid;}
		}
		/// <summary>
		/// ��ʱID����#�ֿ���
		/// </summary>
		public string PayIDtemp
		{
			set{ _payidtemp=value;}
			get{return _payidtemp;}
		}
		#endregion Model

	}
}

