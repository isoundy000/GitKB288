using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Reply ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Reply
	{
		public Reply()
		{}
		#region Model
		private int _id;
		private int _floor;
		private int _forumid;
		private int _bid;
		private int _usid;
		private string _usname;
		private string _content;
		private int _filenum;
		private int _isgood;
		private int _istop;
		private int _isdel;
		private int _replyid;
		private string _restats;
		private DateTime _addtime;
		private string _centtext;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ¥����
		/// </summary>
		public int Floor
		{
			set{ _floor=value;}
			get{return _floor;}
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
		/// ����ID
		/// </summary>
		public int Bid
		{
			set{ _bid=value;}
			get{return _bid;}
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
		/// �ظ�����
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// �ļ�������
		/// </summary>
		public int FileNum
		{
			set{ _filenum=value;}
			get{return _filenum;}
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
		/// �Ƿ��ö�
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// �Ƿ���ɾ��
		/// </summary>
		public int IsDel
		{
			set{ _isdel=value;}
			get{return _isdel;}
		}
		/// <summary>
		/// �ظ�¥��ID
		/// </summary>
		public int ReplyId
		{
			set{ _replyid=value;}
			get{return _replyid;}
		}
		/// <summary>
		/// ��ϸ����
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
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
		/// �ñ�����
		/// </summary>
		public string CentText
		{
			set{ _centtext=value;}
			get{return _centtext;}
		}
		#endregion Model

	}
}

