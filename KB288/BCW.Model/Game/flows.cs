using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����flows ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class flows
	{
		public flows()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _zid;
		private string _ztitle;
		private int _cnum;
		private int _water;
		private int _worm;
		private int _weeds;
		private int _state;
        private int _cnum2;
		private int _checkuid;
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
		public int zid
		{
			set{ _zid=value;}
			get{return _zid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public string ztitle
		{
			set{ _ztitle=value;}
			get{return _ztitle;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int cnum
		{
			set{ _cnum=value;}
			get{return _cnum;}
		}
		/// <summary>
		/// ˮ��
		/// </summary>
		public int water
		{
			set{ _water=value;}
			get{return _water;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int worm
		{
			set{ _worm=value;}
			get{return _worm;}
		}
		/// <summary>
		/// �Ӳ�����
		/// </summary>
		public int weeds
		{
			set{ _weeds=value;}
			get{return _weeds;}
		}
		/// <summary>
        /// ״̬(0δ��ʼ/1����(60����)/2��ѿ(120����)/3����(120����)/4����)
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// ʵ�ʲ���
		/// </summary>
        public int cnum2
		{
            set { _cnum2 = value; }
            get { return _cnum2; }
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int checkuid
		{
			set{ _checkuid=value;}
			get{return _checkuid;}
		}
        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
		#endregion Model

	}
}

