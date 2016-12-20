using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionAnswer ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionAnswer
	{
		public tb_QuestionAnswer()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private int? _questid;
		private string _questtion;
		private string _answer;
		private int? _istrue;
		private int? _ishit;
		private int? _isget;
		private string _gettype;
		private long _getgold;
		private int? _isdone;
		private DateTime? _addtime;
		private int? _needtime;
		private int? _isover;
		private int? _ident;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? usid
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string usname
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int? questID
		{
			set{ _questid=value;}
			get{return _questid;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string questtion
		{
			set{ _questtion=value;}
			get{return _questtion;}
		}
		/// <summary>
		/// �ش�
		/// </summary>
		public string answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// �Ƿ���ȷ
		/// </summary>
		public int? isTrue
		{
			set{ _istrue=value;}
			get{return _istrue;}
		}
		/// <summary>
		/// �Ƿ�����
		/// </summary>
		public int? isHit
		{
			set{ _ishit=value;}
			get{return _ishit;}
		}
		/// <summary>
		/// �Ƿ��õĽ���ID 0����
		/// </summary>
		public int? isGet
		{
			set{ _isget=value;}
			get{return _isget;}
		}
		/// <summary>
		/// ��õ�����
		/// </summary>
		public string getType
		{
			set{ _gettype=value;}
			get{return _gettype;}
		}
		/// <summary>
		/// ��õı�ֵ
		/// </summary>
		public long getGold
		{
			set{ _getgold=value;}
			get{return _getgold;}
		}
		/// <summary>
		/// �Ƿ�ش��� 0��1�ѻش�
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// �ش�����
		/// </summary>
		public int? needTime
		{
			set{ _needtime=value;}
			get{return _needtime;}
		}
		/// <summary>
		/// �Ƿ�ʱ
		/// </summary>
		public int? isOver
		{
			set{ _isover=value;}
			get{return _isover;}
		}
		/// <summary>
		/// ��¼��ʶ
		/// </summary>
		public int? ident
		{
			set{ _ident=value;}
			get{return _ident;}
		}
		#endregion Model

	}
}

