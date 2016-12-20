using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类Mebook 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Mebook
	{
		public Mebook()
		{}
		#region Model
		private int _id;
		private int _usid;
		private int _mid;
		private string _mname;
		private string _mcontent;
		private int _istop;
		private DateTime _addtime;
		private string _retext;
        private int _type;
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
		/// 发表ID
		/// </summary>
		public int MID
		{
			set{ _mid=value;}
			get{return _mid;}
		}
		/// <summary>
		/// 发表昵称
		/// </summary>
		public string MName
		{
			set{ _mname=value;}
			get{return _mname;}
		}
		/// <summary>
		/// 内容
		/// </summary>
		public string MContent
		{
			set{ _mcontent=value;}
			get{return _mcontent;}
		}
		/// <summary>
		/// 是否置顶
		/// </summary>
		public int IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// 发表时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 回复内容
		/// </summary>
		public string ReText
		{
			set{ _retext=value;}
			get{return _retext;}
		}
        /// <summary>
        /// 类型 默认为0，农场为1001，邵广林20160520 增加字段
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model

    }
}

