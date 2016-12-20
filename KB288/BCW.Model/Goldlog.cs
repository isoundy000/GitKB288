using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Goldlog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Goldlog
	{
		public Goldlog()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _purl;
		private int _usid;
		private string _usname;
		private long _acgold;
		private long _aftergold;
		private string _actext;
		private DateTime _addtime;
		private int _bbtag;
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
		/// 提交地址
		/// </summary>
		public string PUrl
		{
			set{ _purl=value;}
			get{return _purl;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UsId
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
        /// <summary>
        /// 操作币
        /// </summary>
        public long AcGold
        {
            set { _acgold = value; }
            get { return _acgold; }
        }
		/// <summary>
		/// 更新后币数量
		/// </summary>
		public long AfterGold
		{
			set{ _aftergold=value;}
			get{return _aftergold;}
		}
        /// <summary>
        /// 操作描述
        /// </summary>
        public string AcText
        {
            set {_actext=value;}
            get {return _actext;}
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
		/// 0-1前台消费,1-2后台消费
		/// </summary>
		public int BbTag
		{
			set{ _bbtag=value;}
			get{return _bbtag;}
		}
		#endregion Model

	}
}

