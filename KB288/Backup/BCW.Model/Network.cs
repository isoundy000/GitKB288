using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Network ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Network
	{
		public Network()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _usid;
		private string _usname;
		private DateTime _overtime;
		private DateTime _addtime;
		private string _onids;
		private int _isubb;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����0�����������û���ʱ/1����/2Ȧ�ı�ʶ
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �㲥����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �����û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �����û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime OverTime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ����Ȧ�����ѵ�ID
		/// </summary>
		public string OnIDs
		{
			set{ _onids=value;}
			get{return _onids;}
		}
		/// <summary>
		/// �Ƿ�֧��UBB
		/// </summary>
		public int IsUbb
		{
			set{ _isubb=value;}
			get{return _isubb;}
		}
		#endregion Model

	}
}

