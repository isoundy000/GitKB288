using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Panel ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Panel
	{
		public Panel()
		{}
		#region Model
		private int _id;
		private string _title;
		private string _purl;
		private int _usid;
		private int _isbr;
		private int _paixu;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ���ӵ�ַ
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �Ƿ���
		/// </summary>
		public int IsBr
		{
			set{ _isbr=value;}
			get{return _isbr;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public int Paixu
		{
			set{ _paixu=value;}
			get{return _paixu;}
		}
		#endregion Model

	}
}

