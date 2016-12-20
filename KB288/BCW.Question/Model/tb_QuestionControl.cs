using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionControl ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionControl
	{
		public tb_QuestionControl()
		{}
		#region Model
		private int _id;
		private int? _usid;
		private string _usname;
		private string _qlist;
		private int? _qcount;
		private int? _qnow;
		private int? _istrue;
		private int? _isfalse;
		private string _answerlist;
		private string _answerjudge;
		private DateTime? _addtime;
		private DateTime? _overtime;
		private int? _isover;
		private int? _isdone;
		private string _remark;
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
		/// ����ID��
		/// </summary>
		public string qList
		{
			set{ _qlist=value;}
			get{return _qlist;}
		}
		/// <summary>
		/// ��Ҫ�ش����������
		/// </summary>
		public int? qCount
		{
			set{ _qcount=value;}
			get{return _qcount;}
		}
		/// <summary>
		/// ��ǰ����ID
		/// </summary>
		public int? qNow
		{
			set{ _qnow=value;}
			get{return _qnow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isTrue
		{
			set{ _istrue=value;}
			get{return _istrue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isFalse
		{
			set{ _isfalse=value;}
			get{return _isfalse;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string answerList
		{
			set{ _answerlist=value;}
			get{return _answerlist;}
		}
		/// <summary>
		/// �ش��ж�
		/// </summary>
		public string answerJudge
		{
			set{ _answerjudge=value;}
			get{return _answerjudge;}
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
		/// 
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
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
		/// 
		/// </summary>
		public int? isDone
		{
			set{ _isdone=value;}
			get{return _isdone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

