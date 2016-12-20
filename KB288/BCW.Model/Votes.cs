using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Votes ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Votes
	{
		public Votes()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _usid;
		private string _title;
		private string _content;
		private string _vote;
		private string _addvote;
		private string _voteid;
		private int _votetype;
		private int _voteleven;
		private int _votetiple;
		private string _restats;
		private int _readcount;
		private int _status;
		private DateTime _voteextime;
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
		/// ���ͣ�0Ϊϵͳ����/��0Ϊ��Ա�����ڷ�����
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
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
		/// ͶƱ����
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
		/// ͶƱѡ���#�ֿ���
		/// </summary>
		public string Vote
		{
			set{ _vote=value;}
			get{return _vote;}
		}
		/// <summary>
		/// ��ӦͶƱ��
		/// </summary>
		public string AddVote
		{
			set{ _addvote=value;}
			get{return _addvote;}
		}
		/// <summary>
		/// ͶƱID
		/// </summary>
		public string VoteID
		{
			set{ _voteid=value;}
			get{return _voteid;}
		}
		/// <summary>
		/// ͶƱ����
		/// </summary>
		public int VoteType
		{
			set{ _votetype=value;}
			get{return _votetype;}
		}
		/// <summary>
		/// ͶƱ�ȼ�
		/// </summary>
		public int VoteLeven
		{
			set{ _voteleven=value;}
			get{return _voteleven;}
		}
		/// <summary>
		/// ͶƱ����
		/// </summary>
		public int VoteTiple
		{
			set{ _votetiple=value;}
			get{return _votetiple;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string ReStats
		{
			set{ _restats=value;}
			get{return _restats;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int Readcount
		{
			set{ _readcount=value;}
			get{return _readcount;}
		}
		/// <summary>
		/// ״̬
		/// </summary>
		public int Status
		{
		    set { _status=value;}
		    get { return _status;}
		}
		/// <summary>
		/// ��ֹͶƱʱ��
		/// </summary>
		public DateTime VoteExTime
		{
			set{ _voteextime=value;}
			get{return _voteextime;}
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

