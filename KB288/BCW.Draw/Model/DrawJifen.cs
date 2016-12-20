using System;
namespace BCW.Draw.Model
{
	/// <summary>
	/// 实体类DrawJifen 。(属性说明自动提取数据库字段的描述信息)
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
		/// 标识ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}

		/// <summary>
		/// 抽奖值
		/// </summary>
		public int Jifen
		{
			set{ _jifen=value;}
			get{return _jifen;}
		}
        /// <summary>
        /// 签到标识0当日未签到，1当日已签到
        /// </summary>
        public int Qd
        {
            set { _Qd = value; }
            get { return _Qd; }
        }
        /// <summary>
        /// 连续签到
        /// </summary>
        public int Qdweek
        {
            set { _Qdweek = value; }
            get { return _Qdweek; }
        }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime  QdTime
        {
            set { _QdTime = value; }
            get { return _QdTime; }
        }
		#endregion Model

	}
}

