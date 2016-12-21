using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����ShuBbs ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ShuBbs
	{
		public ShuBbs()
		{}
		#region Model
		private int _id;
		private int _nid;
		private int _usid;
		private string _usname;
		private string _content;
		private DateTime _addtime;
		private string _addusip;
		private string _retext;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �鱾ID
		/// </summary>
		public int nid
		{
			set{ _nid=value;}
			get{return _nid;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string usname
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// �ύʱ��
		/// </summary>
		public DateTime addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �ύIP
		/// </summary>
		public string addusip
		{
			set{ _addusip=value;}
			get{return _addusip;}
		}
		/// <summary>
		/// �ظ����ݣ����ã�
		/// </summary>
		public string retext
		{
			set{ _retext=value;}
			get{return _retext;}
		}
		#endregion Model

	}
}

