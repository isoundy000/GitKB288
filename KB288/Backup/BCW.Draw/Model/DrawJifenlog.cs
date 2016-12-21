using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// ʵ����Goldlog ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class DrawJifenlog
	{
        public DrawJifenlog()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _purl;
		private int _usid;
		private string _usname;
		private long _acgold;
		private long _aftergold;
		private string _actext;
		private DateTime _addtime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ���ͣ�0��/1Ԫ��
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// �ύ��ַ
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
        /// <summary>
        /// ������
        /// </summary>
        public long AcGold
        {
            set { _acgold = value; }
            get { return _acgold; }
        }
		/// <summary>
		/// ���º������
		/// </summary>
		public long AfterGold
		{
			set{ _aftergold=value;}
			get{return _aftergold;}
		}
        /// <summary>
        /// ��������
        /// </summary>
        public string AcText
        {
            set {_actext=value;}
            get {return _actext;}
        }
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

