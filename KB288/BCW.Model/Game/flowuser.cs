using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类flowuser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class flowuser
	{
		public flowuser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
        private int _iflows;
		private int _score;
		private int _score2;
		private int _score3;
        private int _score4;
        private int _score5;
		private string _flowstat;
		private DateTime _addtime;
        private int _ibw;
        private DateTime _bwtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 会员ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 会员昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
        /// <summary>
        /// 花盆数
        /// </summary>
        public int iFlows
        {
            set { _iflows = value; }
            get { return _iflows; }
        }
		/// <summary>
		/// 技能积分
		/// </summary>
		public int Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 风采积分
		/// </summary>
		public int Score2
		{
			set{ _score2=value;}
			get{return _score2;}
		}
		/// <summary>
        /// 花产量
		/// </summary>
		public int Score3
		{
			set{ _score3=value;}
			get{return _score3;}
		}
        /// <summary>
        /// 送出花篮量
        /// </summary>
        public int Score4
        {
            set { _score4 = value; }
            get { return _score4; }
        }
        /// <summary>
        /// 收到花篮量
        /// </summary>
        public int Score5
        {
            set { _score5 = value; }
            get { return _score5; }
        }

		/// <summary>
		/// 花篮数据
		/// </summary>
		public string FlowStat
		{
			set{ _flowstat=value;}
			get{return _flowstat;}
		}
		/// <summary>
		/// 记录时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        /// <summary>
        /// 今天被玩次数
        /// </summary>
        public int iBw
        {
            set { _ibw = value; }
            get { return _ibw; }
        }
        /// <summary>
        /// 被玩时间
        /// </summary>
        public DateTime BwTime
        {
            set { _bwtime = value; }
            get { return _bwtime; }
        }
		#endregion Model

	}
}

