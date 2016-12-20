using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// 实体类flowmyprop 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class flowmyprop
	{
		public flowmyprop()
		{}
		#region Model
		private int _id;
		private int _did;
		private string _title;
		private int _dnum;
		private DateTime _extime;
        private int _usid;
        private int _znum;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 道具ID
		/// </summary>
		public int did
		{
			set{ _did=value;}
			get{return _did;}
		}
		/// <summary>
		/// 道具名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 道具数量
		/// </summary>
		public int dnum
		{
			set{ _dnum=value;}
			get{return _dnum;}
		}
		/// <summary>
		/// (使用后，截止使用时间)
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 使用了多少个增产肥料
        /// </summary>
        public int znum
        {
            set { _znum = value; }
            get { return _znum; }
        }
		#endregion Model

	}
}

