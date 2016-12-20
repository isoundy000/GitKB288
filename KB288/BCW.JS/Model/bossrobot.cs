using System;
namespace BCW.JS.Model
{
	/// <summary>
	/// 实体类bossrobot 。(属性说明自动提取数据库字段的描述信息)
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
		/// 游戏名字
		/// </summary>
		public string GameName
		{
			set{ _gamename=value;}
			get{return _gamename;}
		}
		/// <summary>
		/// 游戏ID
		/// </summary>
		public int GameID
		{
			set{ _gameid=value;}
			get{return _gameid;}
		}
		/// <summary>
		/// 出动个数
		/// </summary>
		public int robotnum
		{
			set{ _robotnum=value;}
			get{return _robotnum;}
		}
		/// <summary>
		/// 0不勾1勾选
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
        /// <summary>
        /// 游戏XML
        /// </summary>
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }
        /// <summary>
        /// 字段
        /// </summary>
        public string ziduan
        {
            set { _ziduan = value; }
            get { return _ziduan; }
        }
        #endregion Model

    }
}

