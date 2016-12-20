using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Lucklist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Lucklist
	{
		public Lucklist()
		{}
		#region Model
		private int _id;
		private int _sumnum;
		private string _postnum;
		private DateTime _begintime;
		private DateTime _endtime;
		private long _luckcent;
		private long _pool;
		private long _beforepool;
		private int _state;
        private string _panduan;
        private int _bjkl8qihao;
        private string _bjkl8nums;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// ��������8����
        /// </summary>
        public int Bjkl8Qihao
        {
            set { _bjkl8qihao = value; }
            get { return _bjkl8qihao; }
        }
        /// <summary>
        /// ��������8��������
        /// </summary>
        public string Bjkl8Nums
        {
            set { _bjkl8nums = value; }
            get { return _bjkl8nums; }
        }
        /// <summary>
        /// �ж��Ƕ�����������ʶ
        /// </summary>
        public string panduan
        {
            set { _panduan = value; }
            get { return _panduan; }
        }
		/// <summary>
		/// �����������
		/// </summary>
		public int SumNum
		{
			set{ _sumnum=value;}
			get{return _sumnum;}
		}
		/// <summary>
		/// �����������
		/// </summary>
		public string PostNum
		{
			set{ _postnum=value;}
			get{return _postnum;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// ����������ע�ܱ�
		/// </summary>
		public long LuckCent
		{
			set{ _luckcent=value;}
			get{return _luckcent;}
		}
		/// <summary>
		/// ���ػ���
		/// </summary>
		public long Pool
		{
			set{ _pool=value;}
			get{return _pool;}
		}
		/// <summary>
		/// ���ڽ�����ջ���
		/// </summary>
		public long BeforePool
		{
			set{ _beforepool=value;}
			get{return _beforepool;}
		}
		/// <summary>
		/// ״̬ 0δ��/1�ѿ���
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

