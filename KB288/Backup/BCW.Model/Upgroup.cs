using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Upgroup ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Upgroup
	{
		public Upgroup()
		{}
		#region Model
		private int _id;
		private int _leibie;
		private int _types;
		private int _posttype;
		private string _title;
		private string _node;
		private int _usid;
		private int _isreview;
		private int _paixu;
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
		/// ���ͣ�0�ռ�/1���/2����/3��Ƶ/4��Դ��
		/// </summary>
		public int Leibie
		{
			set{ _leibie=value;}
			get{return _leibie;}
		}
		/// <summary>
		/// �������ʣ�0����/1���ѿɼ�/2˽�У�
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �༯����
		/// </summary>
		public int PostType
		{
			set{ _posttype=value;}
			get{return _posttype;}
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
		/// �����ע
		/// </summary>
		public string Node
		{
			set{ _node=value;}
			get{return _node;}
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
		/// �û�ID
		/// </summary>
		public int IsReview
		{
			set{ _isreview=value;}
			get{return _isreview;}
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

