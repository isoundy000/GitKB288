using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����tb_QuestionController ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class tb_QuestionController
	{
		public tb_QuestionController()
		{}
		#region Model
		private int _id;
		private int? _muid;
		private string _uid;
		private int? _type;
		private int? _count;
		private string _list;
		private string _uided;
		private int? _acount;
		private int? _wcount;
		private string _ycount;
		private string _ubbtext;
		private int? _passtime;
		private int? _awardtype;
		private int? _awardptype;
		private int? _award;
		private DateTime? _addtime;
		private DateTime? _overtime;
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
		/// ������ID
		/// </summary>
		public int? muid
		{
			set{ _muid=value;}
			get{return _muid;}
		}
		/// <summary>
		/// ��¼���Իش����η���������Ļ�ԱID #�ָ�
		/// </summary>
		public string uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// 0���л�Ա�ɴ� 1���߻�Ա�� 3�����Ա�� 4ָ����Ա��
		/// </summary>
		public int? type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// �����������
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// ����ID��¼
		/// </summary>
		public string List
		{
			set{ _list=value;}
			get{return _list;}
		}
		/// <summary>
		/// ��¼�������ԱID
		/// </summary>
		public string uided
		{
			set{ _uided=value;}
			get{return _uided;}
		}
		/// <summary>
		/// ��¼�ܵ�����
		/// </summary>
		public int? acount
		{
			set{ _acount=value;}
			get{return _acount;}
		}
		/// <summary>
		/// ��¼δ�������
		/// </summary>
		public int? wcount
		{
			set{ _wcount=value;}
			get{return _wcount;}
		}
		/// <summary>
		/// ��¼�Ѵ������
		/// </summary>
		public string ycount
		{
			set{ _ycount=value;}
			get{return _ycount;}
		}
		/// <summary>
		/// ���͵��������
		/// </summary>
		public string ubbtext
		{
			set{ _ubbtext=value;}
			get{return _ubbtext;}
		}
		/// <summary>
		/// �����������ʱ��
		/// </summary>
		public int? passtime
		{
			set{ _passtime=value;}
			get{return _passtime;}
		}
		/// <summary>
		/// ������ʽ  "0ȫ���н�(���۶Բ���,ֻҪȫ����)", "1ȫ��Բ��н�(ȫ��)", "2���˾��н�(���۶Բ���)
		/// </summary>
		public int? awardtype
		{
			set{ _awardtype=value;}
			get{return _awardtype;}
		}
		/// <summary>
		/// ���Ž�����ʽ 0�̶����� 1������
		/// </summary>
		public int? awardptype
		{
			set{ _awardptype=value;}
			get{return _awardptype;}
		}
		/// <summary>
		/// ��Χֵ 0-x  ��Ϊ�̶�������ֱ���ɸ�ֵ
		/// </summary>
		public int? award
		{
			set{ _award=value;}
			get{return _award;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime? addtime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime? overtime
		{
			set{ _overtime=value;}
			get{return _overtime;}
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

