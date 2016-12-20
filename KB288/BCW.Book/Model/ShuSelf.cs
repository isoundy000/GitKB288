using System;
namespace Book.Model
{
	/// <summary>
	/// 实体类ShuSelf 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class ShuSelf
	{
		public ShuSelf()
		{}
		#region Model
		private int _id;
		private int _aid;
		private string _name;
		private string _sex;
		private string _city;
		private int _pagenum;
		private string _gxids;
        private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int aid
		{
			set{ _aid=value;}
			get{return _aid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 性别（男或女）
		/// </summary>
		public string sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 城市
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 每页字数
		/// </summary>
		public int pagenum
		{
			set{ _pagenum=value;}
			get{return _pagenum;}
		}
		/// <summary>
		/// 提醒ID
		/// </summary>
		public string gxids
		{
			set{ _gxids=value;}
			get{return _gxids;}
		}
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
		#endregion Model

	}
}

