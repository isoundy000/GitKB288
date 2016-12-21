using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Transfer 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型（0币/1元）
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 转自ID
		/// </summary>
		public int FromId
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// 转自ID昵称
		/// </summary>
		public string FromName
		{
			set{ _fromname=value;}
			get{return _fromname;}
		}
		/// <summary>
		/// 转到ID
		/// </summary>
		public int ToId
		{
			set{ _toid=value;}
			get{return _toid;}
		}
		/// <summary>
		/// 转到ID昵称
		/// </summary>
		public string ToName
		{
			set{ _toname=value;}
			get{return _toname;}
		}
		/// <summary>
		/// 操作币
		/// </summary>
		public long AcCent
		{
			set{ _accent=value;}
			get{return _accent;}
		}
		/// <summary>
		/// 操作时间
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

