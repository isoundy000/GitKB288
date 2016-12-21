using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类flows 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
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
		/// 种子ID
		/// </summary>
		public int zid
		{
			set{ _zid=value;}
			get{return _zid;}
		}
		/// <summary>
		/// 种子名称
		/// </summary>
		public string ztitle
		{
			set{ _ztitle=value;}
			get{return _ztitle;}
		}
		/// <summary>
		/// 产量
		/// </summary>
		public int cnum
		{
			set{ _cnum=value;}
			get{return _cnum;}
		}
		/// <summary>
		/// 水份
		/// </summary>
		public int water
		{
			set{ _water=value;}
			get{return _water;}
		}
		/// <summary>
		/// 虫子数量
		/// </summary>
		public int worm
		{
			set{ _worm=value;}
			get{return _worm;}
		}
		/// <summary>
		/// 杂草数量
		/// </summary>
		public int weeds
		{
			set{ _weeds=value;}
			get{return _weeds;}
		}
		/// <summary>
        /// 状态(0未开始/1种子(60分钟)/2发芽(120分钟)/3生长(120分钟)/4成熟)
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 实际产量
		/// </summary>
        public int cnum2
		{
            set { _cnum2 = value; }
            get { return _cnum2; }
		}
		/// <summary>
		/// 帮收ID
		/// </summary>
		public int checkuid
		{
			set{ _checkuid=value;}
			get{return _checkuid;}
		}
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
		#endregion Model

	}
}

