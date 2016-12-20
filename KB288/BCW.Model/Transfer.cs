using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Transfer ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Transfer
	{
		public Transfer()
		{}
		#region Model
		private int _id;
		private int _types;
		private int _fromid;
		private string _fromname;
		private int _toid;
		private string _toname;
		private long _accent;
		private DateTime _addtime;
        private string _zfbno;
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
		/// ת��ID
		/// </summary>
		public int FromId
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// ת��ID�ǳ�
		/// </summary>
		public string FromName
		{
			set{ _fromname=value;}
			get{return _fromname;}
		}
		/// <summary>
		/// ת��ID
		/// </summary>
		public int ToId
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// ת��ID�ǳ�
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// ������
		/// </summary>
		public long AcCent
		{
			set{ _accent=value;}
			get{return _accent;}
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
        /// zfbno
        /// </summary>
        public string zfbNo
        {
            set { _zfbno = value; }
            get { return _zfbno; }
        }
		#endregion Model

	}
}

