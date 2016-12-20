using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Goldlog ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Goldlog
	{
		public Goldlog()
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
		private int _bbtag;
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
		/// <summary>
		/// 0-1ǰ̨����,1-2��̨����
		/// </summary>
		public int BbTag
		{
			set{ _bbtag=value;}
			get{return _bbtag;}
		}
		#endregion Model

	}
}

