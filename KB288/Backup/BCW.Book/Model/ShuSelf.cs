using System;
namespace Book.Model
{
	/// <summary>
	/// ʵ����ShuSelf ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class ShuSelf
	{
		public ShuSelf()
		{}
		#region Model
		private int _id;
		private int _aid;
		private string _name;
		private string _sex;
		private string _city;
		private int _pagenum;
		private string _gxids;
        private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int aid
		{
			set{ _aid=value;}
			get{return _aid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// �Ա��л�Ů��
		/// </summary>
		public string sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// ÿҳ����
		/// </summary>
		public int pagenum
		{
			set{ _pagenum=value;}
			get{return _pagenum;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public string gxids
		{
			set{ _gxids=value;}
			get{return _gxids;}
		}
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
		#endregion Model

	}
}

