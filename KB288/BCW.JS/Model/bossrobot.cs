using System;
namespace BCW.JS.Model
{
	/// <summary>
	/// ʵ����bossrobot ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class bossrobot
	{
		public bossrobot()
		{}
		#region Model
		private int _id;
		private string _gamename;
		private int _gameid;
		private int _robotnum;
		private int _type;
        private string _xml;
        private string _ziduan;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��Ϸ����
		/// </summary>
		public string GameName
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// ��ϷID
		/// </summary>
		public int GameID
		{
			set{ _gameid=value;}
			get{return _gameid;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int robotnum
		{
			set{ _robotnum=value;}
			get{return _robotnum;}
		}
		/// <summary>
		/// 0����1��ѡ
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
        /// <summary>
        /// ��ϷXML
        /// </summary>
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }
        /// <summary>
        /// �ֶ�
        /// </summary>
        public string ziduan
        {
            set { _ziduan = value; }
            get { return _ziduan; }
        }
        #endregion Model

    }
}

