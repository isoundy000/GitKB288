using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionsList ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionsList
	{
		public tb_QuestionsList()
		{}
		#region Model
		private int _id;
		private string _question;
		private string _choosea;
		private string _chooseb;
		private string _choosec;
		private string _choosed;
		private string _answer;
		private string _styleid;
		private int? _style;
		private string _type;
		private int? _deficult;
		private int? _awardid;
		private string _awardtype;
		private int? _awardgold;
		private string _title;
		private string _img;
		private int? _hit;
		private int? _statue;
		private int? _trueanswer;
		private int? _falseanswer;
		private string _remarks;
		private string _indent;
		private DateTime? _addtime;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string question
		{
			set{ _question=value;}
			get{return _question;}
		}
		/// <summary>
		/// Aѡ��
		/// </summary>
		public string chooseA
		{
			set{ _choosea=value;}
			get{return _choosea;}
		}
		/// <summary>
		/// Bѡ��
		/// </summary>
		public string chooseB
		{
			set{ _chooseb=value;}
			get{return _chooseb;}
		}
		/// <summary>
		/// Cѡ��
		/// </summary>
		public string chooseC
		{
			set{ _choosec=value;}
			get{return _choosec;}
		}
		/// <summary>
		/// Dѡ��
		/// </summary>
		public string chooseD
		{
			set{ _choosed=value;}
			get{return _choosed;}
		}
		/// <summary>
		/// ������ȷ��
		/// </summary>
		public string answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public string styleID
		{
			set{ _styleid=value;}
			get{return _styleid;}
		}
		/// <summary>
		/// ��Ŀ����:1ѡ����(4ѡ1)2(2ѡ1)3(3ѡ1)
		/// </summary>
		public int? style
		{
			set{ _style=value;}
			get{return _style;}
		}
		/// <summary>
		/// ˵��
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// �Ѷ�
		/// </summary>
		public int? deficult
		{
			set{ _deficult=value;}
			get{return _deficult;}
		}
		/// <summary>
		/// ���н�ƷID(0�򲻴���)
		/// </summary>
		public int? awardId
		{
			set{ _awardid=value;}
			get{return _awardid;}
		}
		/// <summary>
		/// ���н�Ʒ����
		/// </summary>
		public string awardType
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? awardGold
		{
			set{ _awardgold=value;}
			get{return _awardgold;}
		}
		/// <summary>
		/// ��Ӧ����
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �Ƿ���ͼƬ(0����)
		/// </summary>
		public string img
		{
			set{ _img=value;}
			get{return _img;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public int? hit
		{
			set{ _hit=value;}
			get{return _hit;}
		}
		/// <summary>
		/// ����״̬(1��ѡ0����ѡ)
		/// </summary>
		public int? statue
		{
			set{ _statue=value;}
			get{return _statue;}
		}
		/// <summary>
		/// ��ȷ
		/// </summary>
		public int? trueAnswer
		{
			set{ _trueanswer=value;}
			get{return _trueanswer;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int? falseAnswer
		{
			set{ _falseanswer=value;}
			get{return _falseanswer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string indent
		{
			set{ _indent=value;}
			get{return _indent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

