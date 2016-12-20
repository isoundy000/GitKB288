using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionsAnswerCtr ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionsAnswerCtr
	{
		public tb_QuestionsAnswerCtr()
		{}
		#region Model
		private int _id;
		private int? _uid;
		private int? _contrid;
		private string _list;
		private string _answerlist;
		private int? _count;
		private int? _now;
		private int? _awardtype;
		private int? _awardid;
		private string _explain;
		private int? _awardgold;
		private int? _truecount;
		private int? _flasecount;
		private int? _ishit;
		private int? _isdone;
		private DateTime? _addtime;
		private DateTime? _overtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �û�id
		/// </summary>
		public int? uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// ���������id
		/// </summary>
		public int? contrID
		{
			set{ _contrid=value;}
			get{return _contrid;}
		}
		/// <summary>
		/// ����id��
		/// </summary>
		public string List
		{
			set{ _list=value;}
			get{return _list;}
		}
		/// <summary>
		/// �ѻش�id��
		/// </summary>
		public string answerList
		{
			set{ _answerlist=value;}
			get{return _answerlist;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// ��ǰ�ش���
		/// </summary>
		public int? now
		{
			set{ _now=value;}
			get{return _now;}
		}
		/// <summary>
		/// ��Ʒ����
		/// </summary>
		public int? awardtype
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// ��Ʒid
		/// </summary>
		public int? awardId
		{
			set{ _awardid=value;}
			get{return _awardid;}
		}
		/// <summary>
		/// ˵��
		/// </summary>
		public string explain
		{
			set{ _explain=value;}
			get{return _explain;}
		}
		/// <summary>
		/// ��ñ�ֵ
		/// </summary>
		public int? awardgold
		{
			set{ _awardgold=value;}
			get{return _awardgold;}
		}
		/// <summary>
		/// ��Դ���
		/// </summary>
		public int? trueCount
		{
			set{ _truecount=value;}
			get{return _truecount;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public int? flaseCount
		{
			set{ _flasecount=value;}
			get{return _flasecount;}
		}
		/// <summary>
		/// �Ƿ�����
		/// </summary>
		public int? ishit
		{
			set{ _ishit=value;}
			get{return _ishit;}
		}
		/// <summary>
		/// �Ƿ����
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// �ش�ʱ��
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
		}
		#endregion Model

	}
}

