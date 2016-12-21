using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����FComment ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class FComment
	{
		public FComment()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _detailid;
		private string _content;
		private int _usid;
		private string _usname;
		private string _addusip;
		private DateTime _addtime;
		private string _retext;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���ͣ�1/��־/2��ᣩ
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ������ID
		/// </summary>
		public int DetailId
		{
			set{ _detailid=value;}
			get{return _detailid;}
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
		/// �ύIP
		/// </summary>
		public string AddUsIP
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// �ύʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �ظ�����
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		#endregion Model

	}
}

