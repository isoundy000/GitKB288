using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����flowuser ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class flowuser
	{
		public flowuser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
        private int _iflows;
		private int _score;
		private int _score2;
		private int _score3;
        private int _score4;
        private int _score5;
		private string _flowstat;
		private DateTime _addtime;
        private int _ibw;
        private DateTime _bwtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��ԱID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// ��Ա�ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
        /// <summary>
        /// ������
        /// </summary>
        public int iFlows
        {
            set { _iflows = value; }
            get { return _iflows; }
        }
		/// <summary>
		/// ���ܻ���
		/// </summary>
		public int Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// ��ɻ���
		/// </summary>
		public int Score2
		{
			set{ _score2=value;}
			get{return _score2;}
		}
		/// <summary>
        /// ������
		/// </summary>
		public int Score3
		{
			set{ _score3=value;}
			get{return _score3;}
		}
        /// <summary>
        /// �ͳ�������
        /// </summary>
        public int Score4
        {
            set { _score4 = value; }
            get { return _score4; }
        }
        /// <summary>
        /// �յ�������
        /// </summary>
        public int Score5
        {
            set { _score5 = value; }
            get { return _score5; }
        }

		/// <summary>
		/// ��������
		/// </summary>
		public string FlowStat
		{
			set{ _flowstat=value;}
			get{return _flowstat;}
		}
		/// <summary>
		/// ��¼ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
        /// ���챻�����
        /// </summary>
        public int iBw
        {
            set { _ibw = value; }
            get { return _ibw; }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime BwTime
        {
            set { _bwtime = value; }
            get { return _bwtime; }
        }
		#endregion Model

	}
}

