using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// ʵ����DrawJifen ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DrawJifen
	{
		public DrawJifen()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _jifen;
        private int _Qd;
        private int _Qdweek;
        private DateTime _QdTime;
		/// <summary>
		/// ��ʶID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		/// �齱ֵ
		/// </summary>
		public int Jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
        /// <summary>
        /// ǩ����ʶ0����δǩ����1������ǩ��
        /// </summary>
        public int Qd
        {
            set { _Qd = value; }
            get { return _Qd; }
        }
        /// <summary>
        /// ����ǩ��
        /// </summary>
        public int Qdweek
        {
            set { _Qdweek = value; }
            get { return _Qdweek; }
        }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        public DateTime  QdTime
        {
            set { _QdTime = value; }
            get { return _QdTime; }
        }
		#endregion Model

	}
}

