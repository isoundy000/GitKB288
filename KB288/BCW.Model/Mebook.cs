using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Mebook ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Mebook
	{
		public Mebook()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _mid;
		private string _mname;
		private string _mcontent;
		private int _istop;
		private DateTime _addtime;
		private string _retext;
        private int _type;
        /// <summary>
        /// ����ID
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
		/// ����ID
		/// </summary>
		public int MID
		{
			set{ _mid=value;}
			get{return _mid;}
		}
		/// <summary>
		/// �����ǳ�
		/// </summary>
		public string MName
		{
			set{ _mname=value;}
			get{return _mname;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public string MContent
		{
			set{ _mcontent=value;}
			get{return _mcontent;}
		}
		/// <summary>
		/// �Ƿ��ö�
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
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
		/// �ظ�����
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
        /// <summary>
        /// ���� Ĭ��Ϊ0��ũ��Ϊ1001���۹���20160520 �����ֶ�
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

