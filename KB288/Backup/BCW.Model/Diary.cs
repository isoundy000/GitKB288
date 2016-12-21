using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Diary ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Diary
	{
		public Diary()
		{}
		#region Model
		private int _id;
		private int _nodeid;
		private string _title;
		private string _weather;
		private string _content;
		private int _usid;
		private string _usname;
		private int _istop;
		private int _isgood;
		private int _replynum;
		private int _readnum;
		private string _addusip;
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
		/// �ռǷ���ID
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// �ռǱ���
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string Weather
		{
			set{ _weather=value;}
			get{return _weather;}
		}
		/// <summary>
		/// �ռ�����
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
		/// �Ƿ��ö�
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// �Ƿ񾫻�
		/// </summary>
		public int IsGood
		{
			set{ _isgood=value;}
			get{return _isgood;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public int ReplyNum
		{
			set{ _replynum=value;}
			get{return _replynum;}
		}
		/// <summary>
		/// �Ķ���
		/// </summary>
		public int ReadNum
		{
			set{ _readnum=value;}
			get{return _readnum;}
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
		#endregion Model

	}
}

