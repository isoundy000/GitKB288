using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����flowmyprop ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class flowmyprop
	{
		public flowmyprop()
		{}
		#region Model
		private int _id;
		private int _did;
		private string _title;
		private int _dnum;
		private DateTime _extime;
        private int _usid;
        private int _znum;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int did
		{
			set{ _did=value;}
			get{return _did;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int dnum
		{
			set{ _dnum=value;}
			get{return _dnum;}
		}
		/// <summary>
		/// (ʹ�ú󣬽�ֹʹ��ʱ��)
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
        /// <summary>
        /// �û�ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// ʹ���˶��ٸ���������
        /// </summary>
        public int znum
        {
            set { _znum = value; }
            get { return _znum; }
        }
		#endregion Model

	}
}

